using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsNamespaceGenerator : IAdapterTestsNamespaceGenerator {
    private readonly IAdapterTestsClassGenerator _classGenerator;

    public AdapterTestsNamespaceGenerator(IAdapterTestsClassGenerator classGenerator) {
      if (classGenerator == null) throw new ArgumentNullException(nameof(classGenerator));
      _classGenerator = classGenerator;
    }

    public NamespaceDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithCompilationUnit context) {
      var @namespace = NamespaceDeclaration(context.Adapter.CompilationUnitSyntax.Members
        .OfType<NamespaceDeclarationSyntax>().Single().Name);
      return @namespace.WithMembers(
        SingletonList<MemberDeclarationSyntax>(_classGenerator.Generate(context.WithNamespace(@namespace))));
    }

    public NamespaceDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithCompilationUnit context) {
      var @namespace = NamespaceDeclaration(context.Adapter.CompilationUnitSyntax.Members
        .OfType<NamespaceDeclarationSyntax>().Single().Name);
      return @namespace.WithMembers(
        SingletonList<MemberDeclarationSyntax>(_classGenerator.Generate(context.WithNamespace(@namespace))));
    }
  }
}