using System;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public class MatchByEnumNameSimilarityStrategy : IEnumMatchingStrategy {
    private readonly INameSimilarityDeterminer _nameSimilarityDeterminer;

    public MatchByEnumNameSimilarityStrategy(INameSimilarityDeterminer nameSimilarityDeterminer) {
      if (nameSimilarityDeterminer == null) throw new ArgumentNullException(nameof(nameSimilarityDeterminer));
      _nameSimilarityDeterminer = nameSimilarityDeterminer;
    }

    public bool Matches(IEnum source, IEnum target) {
      return _nameSimilarityDeterminer.AreSimilar(source.EnumDeclarationSyntax.Identifier.Text,
        target.EnumDeclarationSyntax.Identifier.Text);
    }
  }
}