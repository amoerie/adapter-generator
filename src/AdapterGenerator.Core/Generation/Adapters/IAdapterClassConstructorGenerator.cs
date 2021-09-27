using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdapterClassConstructorGenerator {
    ConstructorDeclarationSyntax Generate(IClassAdapterGenerationContextWithClass context);
  }
}