using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsCompilationUnitGenerator {
    CompilationUnitSyntax Generate(IClassAdapterTestsGenerationContext context);
    CompilationUnitSyntax Generate(IEnumAdapterTestsGenerationContext context);
  }
}