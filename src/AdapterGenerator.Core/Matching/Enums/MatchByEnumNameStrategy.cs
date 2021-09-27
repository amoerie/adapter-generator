using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public class MatchByEnumNameStrategy : IEnumMatchingStrategy {
    public bool Matches(IEnum source, IEnum target) {
      return string.Equals(source.EnumDeclarationSyntax.Identifier.Text, target.EnumDeclarationSyntax.Identifier.Text);
    }
  }
}