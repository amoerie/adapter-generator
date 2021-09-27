using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdapterNamespaceGenerator : IAdapterNamespaceGenerator {
    private readonly IAdapterClassGenerator _classGenerator;

    public AdapterNamespaceGenerator(IAdapterClassGenerator classGenerator) {
      if (classGenerator == null) throw new ArgumentNullException(nameof(classGenerator));
      _classGenerator = classGenerator;
    }

    public NamespaceDeclarationSyntax Generate(IClassAdapterGenerationContextWithCompilationUnit context) {
      var @namespace = NamespaceDeclaration(context.Blueprint.ClassMatch.Target.NamespaceDeclarationSyntax.Name);
      return @namespace
        .WithMembers(
          SingletonList<MemberDeclarationSyntax>(
            _classGenerator.Generate(context.WithNamespace(@namespace))));
    }

    public NamespaceDeclarationSyntax Generate(IEnumAdapterGenerationContextWithCompilationUnit context) {
      var @namespace = NamespaceDeclaration(context.Blueprint.EnumMatch.Target.NamespaceDeclarationSyntax.Name);
      return @namespace
        .WithMembers(
          SingletonList<MemberDeclarationSyntax>(
            _classGenerator.Generate(context.WithNamespace(@namespace))));
    }
  }
}