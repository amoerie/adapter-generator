using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsAdaptMethodTestsGenerator {
    ClassDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context);
    ClassDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithClass context);
  }
}