using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation {
  public interface ITypeSyntaxAnalyzer {
    bool IsArray(TypeSyntax typeSyntax);
    bool IsIEnumerable(TypeSyntax typeSyntax);
    bool IsCollectionType(TypeSyntax typeSyntax);
    bool ImplementsIEnumerable(TypeSyntax typeSyntax);
    TypeSyntax ExtractElementTypeFromEnumerable(TypeSyntax type);
  }
}