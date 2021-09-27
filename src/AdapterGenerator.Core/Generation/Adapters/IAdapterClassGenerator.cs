using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdapterClassGenerator {
    ClassDeclarationSyntax Generate(IClassAdapterGenerationContextWithNamespace context);
    ClassDeclarationSyntax Generate(IEnumAdapterGenerationContextWithNamespace context);
  }
}