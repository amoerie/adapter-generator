using System;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public class MatchByEnumNamesAndPropertiesSimilarityStrategy : IEnumMatchingStrategy {
    private readonly INameSimilarityDeterminer _nameSimilarityDeterminer;
    private readonly IEnumValueMatcher _enumValueMatcher;

    public MatchByEnumNamesAndPropertiesSimilarityStrategy(INameSimilarityDeterminer nameSimilarityDeterminer,
      IEnumValueMatcher enumValueMatcher) {
      if (nameSimilarityDeterminer == null) throw new ArgumentNullException(nameof(nameSimilarityDeterminer));
      if (enumValueMatcher == null) throw new ArgumentNullException(nameof(enumValueMatcher));
      _nameSimilarityDeterminer = nameSimilarityDeterminer;
      _enumValueMatcher = enumValueMatcher;
    }

    public bool Matches(IEnum source, IEnum target) {
      string sourceName = source.EnumDeclarationSyntax.Identifier.Text;
      string targetName = target.EnumDeclarationSyntax.Identifier.Text;
      double numberOfMatchingValues = _enumValueMatcher.Match(source, target).Count;
      return _nameSimilarityDeterminer.AreSimilar(sourceName, targetName) &&
             numberOfMatchingValues/target.Values.Count > 0.5;
    }
  }
}