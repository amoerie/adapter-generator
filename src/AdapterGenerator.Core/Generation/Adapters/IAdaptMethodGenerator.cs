using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdaptMethodGenerator {
    MethodDeclarationSyntax Generate(IClassAdapterGenerationContextWithClass context);
    MethodDeclarationSyntax Generate(IEnumAdapterGenerationContextWithClass context);
  }
}