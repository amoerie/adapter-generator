using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsNamespaceGenerator {
    NamespaceDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithCompilationUnit context);
    NamespaceDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithCompilationUnit context);
  }
}