using Microsoft.CodeAnalysis;

namespace AdapterGenerator.Core.Parsing {
  public interface ICodeToSyntaxTreeParser {
    /// <summary>
    /// Parses the specified code to a syntax tree and returns it.
    /// </summary>
    /// <param name="code">The code.</param>
    SyntaxTree Parse(string code);
  }
}