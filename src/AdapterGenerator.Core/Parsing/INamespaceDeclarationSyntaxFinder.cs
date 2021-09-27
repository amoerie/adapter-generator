using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public interface INamespaceDeclarationSyntaxFinder {
    NamespaceDeclarationSyntax FindNamespace(BaseTypeDeclarationSyntax typeDeclarationSyntax);
  }
}