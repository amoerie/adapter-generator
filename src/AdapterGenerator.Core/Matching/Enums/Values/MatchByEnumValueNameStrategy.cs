using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public class MatchByEnumValueNameStrategy : IEnumValueMatchingStrategy {
    public bool Matches(IEnumValue source, IEnumValue target) {
      return string.Equals(source.EnumMemberDeclarationSyntax.Identifier.Text,
        target.EnumMemberDeclarationSyntax.Identifier.Text);
    }
  }
}