using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdapterNamespaceGenerator {
    NamespaceDeclarationSyntax Generate(IClassAdapterGenerationContextWithCompilationUnit context);
    NamespaceDeclarationSyntax Generate(IEnumAdapterGenerationContextWithCompilationUnit context);
  }
}