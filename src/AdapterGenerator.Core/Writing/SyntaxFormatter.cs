using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;

namespace AdapterGenerator.Core.Writing {
  public class SyntaxFormatter : ISyntaxFormatter {
    public string Format(SyntaxNode syntax) {
      var workspace = new AdhocWorkspace();
      return Formatter.Format(syntax, SyntaxAnnotation.ElasticAnnotation, workspace)
        .NormalizeWhitespace()
        .ToFullString();
    }
  }
}