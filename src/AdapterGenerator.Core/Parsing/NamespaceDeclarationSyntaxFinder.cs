using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class NamespaceDeclarationSyntaxFinder : INamespaceDeclarationSyntaxFinder {
    public NamespaceDeclarationSyntax FindNamespace(BaseTypeDeclarationSyntax classDeclarationSyntax) {
      var parent = classDeclarationSyntax.Parent;
      while (parent != null) {
        var namespaceDeclarationSyntax = parent as NamespaceDeclarationSyntax;
        if (namespaceDeclarationSyntax != null) return namespaceDeclarationSyntax;
        parent = parent.Parent;
      }
      return null;
    }
  }
}