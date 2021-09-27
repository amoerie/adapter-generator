using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsClassGenerator {
    ClassDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithNamespace context);
    ClassDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithNamespace context);
  }
}