using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public interface IPropertyMatchingStrategiesFactory {
    IImmutableList<IPropertyMatchingStrategy> Create(IImmutableList<IClassMatchWithoutPropertyMatches> classMatches,
      IImmutableList<IEnumMatch> enumMatches);
  }

  public class PropertyMatchingStrategiesFactory : IPropertyMatchingStrategiesFactory {
    private readonly INameSimilarityDeterminer _nameSimilarityDeterminer;

    public PropertyMatchingStrategiesFactory(INameSimilarityDeterminer nameSimilarityDeterminer) {
      if (nameSimilarityDeterminer == null) throw new ArgumentNullException(nameof(nameSimilarityDeterminer));
      _nameSimilarityDeterminer = nameSimilarityDeterminer;
    }

    public IImmutableList<IPropertyMatchingStrategy> Create(
      IImmutableList<IClassMatchWithoutPropertyMatches> classMatches, IImmutableList<IEnumMatch> enumMatches) {
      var typeEquivalencyDeterminer = new TypeEquivalencyDeterminer(classMatches, enumMatches);
      return ImmutableList.Create<IPropertyMatchingStrategy>(
        new MatchByPropertyNameAndTypeStrategy(typeEquivalencyDeterminer),
        new MatchByPropertyNameSimilarityAndTypeStrategy(_nameSimilarityDeterminer, typeEquivalencyDeterminer)
        );
    }
  }
}