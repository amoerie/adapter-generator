using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsConstructorTestsGenerator {
    ClassDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context);
    ClassDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithClass context);
  }
}