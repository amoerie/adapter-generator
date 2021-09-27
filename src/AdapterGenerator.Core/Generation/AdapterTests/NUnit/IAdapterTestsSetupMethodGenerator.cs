using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsSetupMethodGenerator {
    MethodDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context);
    MethodDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithClass context);
  }
}