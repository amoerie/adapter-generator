using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdapterCompilationUnitGenerator : IAdapterCompilationUnitGenerator {
    private readonly IAdapterNamespaceGenerator _namespaceGenerator;

    public AdapterCompilationUnitGenerator(IAdapterNamespaceGenerator namespaceGenerator) {
      if (namespaceGenerator == null) throw new ArgumentNullException(nameof(namespaceGenerator));
      _namespaceGenerator = namespaceGenerator;
    }

    private IEnumerable<UsingDirectiveSyntax> CreateUsingDirectives(CompilationUnitSyntax source,
      CompilationUnitSyntax target) {
      var defaultUsingDirectives = new[] {"System", "System.Collections.Generic", "System.Linq"};
      var sourceAndTargetUsingDirectives = source.Usings.AsEnumerable().Concat(target.Usings)
        .Select(usingDirective => usingDirective.Name.ToFullString());
      var usingDirectives = defaultUsingDirectives.Concat(sourceAndTargetUsingDirectives)
        .Distinct()
        .OrderBy(s => s)
        .Select(s => UsingDirective(ParseName(s)));
      return usingDirectives;
    }

    public CompilationUnitSyntax Generate(IClassAdapterGenerationContext context) {
      var compilationUnit = CompilationUnit(
        new SyntaxList<ExternAliasDirectiveSyntax>(),
        new SyntaxList<UsingDirectiveSyntax>().AddRange(
          CreateUsingDirectives(context.Blueprint.ClassMatch.Source.CompilationUnitSyntax,
            context.Blueprint.ClassMatch.Target.CompilationUnitSyntax)),
        new SyntaxList<AttributeListSyntax>(),
        new SyntaxList<MemberDeclarationSyntax>());
      return compilationUnit.WithMembers(List(new MemberDeclarationSyntax[] {
        _namespaceGenerator.Generate(context.WithCompilationUnit(compilationUnit))
      }));
    }

    public CompilationUnitSyntax Generate(IEnumAdapterGenerationContext context) {
      var compilationUnit = CompilationUnit(
        new SyntaxList<ExternAliasDirectiveSyntax>(),
        new SyntaxList<UsingDirectiveSyntax>().AddRange(
          CreateUsingDirectives(context.Blueprint.EnumMatch.Source.CompilationUnitSyntax,
            context.Blueprint.EnumMatch.Target.CompilationUnitSyntax)),
        new SyntaxList<AttributeListSyntax>(),
        new SyntaxList<MemberDeclarationSyntax>());
      return compilationUnit.WithMembers(List(new MemberDeclarationSyntax[] {
        _namespaceGenerator.Generate(context.WithCompilationUnit(compilationUnit))
      }));
    }
  }
}