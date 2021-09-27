using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class QualifiedNameFactory : IQualifiedNameFactory {
    public NameSyntax Create(BaseTypeDeclarationSyntax type) {
      return Create(type as SyntaxNode);
    }

    private NameSyntax Create(SyntaxNode syntaxNode) {
      if (syntaxNode == null) return null;
      var namespaceDeclaration = syntaxNode as NamespaceDeclarationSyntax;
      if (namespaceDeclaration != null)
        return
          namespaceDeclaration.Name.WithLeadingTrivia(SyntaxTriviaList.Empty)
            .WithTrailingTrivia(SyntaxTriviaList.Empty);
      // enum, struct or class
      var baseTypeDeclaration = syntaxNode as BaseTypeDeclarationSyntax;
      if (baseTypeDeclaration != null) {
        var parentName = Create(baseTypeDeclaration.Parent);
        var name = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(baseTypeDeclaration.Identifier.Text));
        return parentName != null ? SyntaxFactory.QualifiedName(parentName, name) as NameSyntax : name;
      }
      return Create(syntaxNode.Parent);
    }
  }
}