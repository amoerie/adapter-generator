using Microsoft.CodeAnalysis;

namespace AdapterGenerator.Core.Parsing {
  public interface ITypeDeclarationsFromSyntaxTreeExtractor {
    /// <summary>
    /// Extracts the classes from a parsed C# file
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    ITypeDeclarations Extract(SyntaxTree syntaxTree);
  }
}