using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public interface ITypeEquivalencyDeterminer {
    bool AreEquivalent(TypeSyntax type, TypeSyntax otherType);
  }
}