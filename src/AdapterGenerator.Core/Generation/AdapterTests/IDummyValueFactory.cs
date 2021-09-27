using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public interface IDummyValueFactory {
    ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, TypeSyntax type);
  }
}