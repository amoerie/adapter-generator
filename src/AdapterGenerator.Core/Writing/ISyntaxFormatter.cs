using Microsoft.CodeAnalysis;

namespace AdapterGenerator.Core.Writing {
  public interface ISyntaxFormatter {
    string Format(SyntaxNode syntax);
  }
}