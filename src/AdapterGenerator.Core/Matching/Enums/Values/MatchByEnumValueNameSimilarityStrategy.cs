using System;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public class MatchByEnumValueNameSimilarityStrategy : IEnumValueMatchingStrategy {
    private readonly INameSimilarityDeterminer _nameSimilarityDeterminer;

    public MatchByEnumValueNameSimilarityStrategy(INameSimilarityDeterminer nameSimilarityDeterminer) {
      if (nameSimilarityDeterminer == null) throw new ArgumentNullException(nameof(nameSimilarityDeterminer));
      _nameSimilarityDeterminer = nameSimilarityDeterminer;
    }

    public bool Matches(IEnumValue source, IEnumValue target) {
      return _nameSimilarityDeterminer.AreSimilar(source.EnumMemberDeclarationSyntax.Identifier.Text,
        target.EnumMemberDeclarationSyntax.Identifier.Text);
    }
  }
}