using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class TypeDeclarationsFromSyntaxTreeExtractor : ITypeDeclarationsFromSyntaxTreeExtractor {
    private readonly ILogger _logger;
    private readonly INamespaceDeclarationSyntaxFinder _namespaceDeclarationSyntaxFinder;

    public TypeDeclarationsFromSyntaxTreeExtractor(ILogger logger,
      INamespaceDeclarationSyntaxFinder namespaceDeclarationSyntaxFinder) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      if (namespaceDeclarationSyntaxFinder == null)
        throw new ArgumentNullException(nameof(namespaceDeclarationSyntaxFinder));
      _logger = logger;
      _namespaceDeclarationSyntaxFinder = namespaceDeclarationSyntaxFinder;
    }

    public ITypeDeclarations Extract(SyntaxTree syntaxTree) {
      var root = syntaxTree.GetCompilationUnitRoot();
      var collector = new DeclarationCollectingSyntaxWalker();
      collector.Visit(root);
      _logger.Info(
        $"Found {collector.Classes.Count} classes: {string.Join(", ", collector.Classes.Select(c => c.Identifier.Text))}");
      _logger.Info(
        $"Found {collector.Enums.Count} enums: {string.Join(", ", collector.Enums.Select(c => c.Identifier.Text))}");
      return new TypeDeclarations {
        Classes = collector.Classes
          .Select(c => (IClass) new RealClass(root, _namespaceDeclarationSyntaxFinder.FindNamespace(c), c))
          .ToImmutableList(),
        Enums = collector.Enums
          .Select(e => (IEnum) new RealEnum(root, _namespaceDeclarationSyntaxFinder.FindNamespace(e), e))
          .ToImmutableList()
      };
    }

    private class DeclarationCollectingSyntaxWalker : CSharpSyntaxWalker {
      private readonly ICollection<ClassDeclarationSyntax> _classes;
      private readonly ICollection<EnumDeclarationSyntax> _enums;

      public DeclarationCollectingSyntaxWalker() {
        _classes = new List<ClassDeclarationSyntax>();
        _enums = new List<EnumDeclarationSyntax>();
      }

      public override void VisitEnumDeclaration(EnumDeclarationSyntax node) {
        base.VisitEnumDeclaration(node);
        _enums.Add(node);
      }

      public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        base.VisitClassDeclaration(node);
        _classes.Add(node);
      }

      public ICollection<ClassDeclarationSyntax> Classes => _classes;
      public ICollection<EnumDeclarationSyntax> Enums => _enums;
    }
  }
}