using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdapterCompilationUnitGenerator {
    /// <summary>
    ///   Creates the root compilation unit for the adapter
    /// </summary>
    CompilationUnitSyntax Generate(IClassAdapterGenerationContext context);

    CompilationUnitSyntax Generate(IEnumAdapterGenerationContext context);
  }
}