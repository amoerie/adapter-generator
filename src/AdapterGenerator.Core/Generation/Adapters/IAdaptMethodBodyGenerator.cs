using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdaptMethodBodyGenerator {
    BlockSyntax Generate(IClassAdapterGenerationContextWithClass context);
    BlockSyntax Generate(IEnumAdapterGenerationContextWithClass context);
  }
}