using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public class MatchByClassNameStrategy : IClassMatchingStrategy {
    public bool Matches(IClass source, IClass target) {
      return string.Equals(source.ClassDeclarationSyntax.Identifier.Text, target.ClassDeclarationSyntax.Identifier.Text);
    }
  }
}