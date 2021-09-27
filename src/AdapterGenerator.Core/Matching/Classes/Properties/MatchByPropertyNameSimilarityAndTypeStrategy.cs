using System;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class MatchByPropertyNameSimilarityAndTypeStrategy : IPropertyMatchingStrategy {
    private readonly INameSimilarityDeterminer _nameSimilarityDeterminer;
    private readonly ITypeEquivalencyDeterminer _typeEquivalencyDeterminer;

    public MatchByPropertyNameSimilarityAndTypeStrategy(INameSimilarityDeterminer nameSimilarityDeterminer,
      ITypeEquivalencyDeterminer typeEquivalencyDeterminer) {
      if (nameSimilarityDeterminer == null) throw new ArgumentNullException(nameof(nameSimilarityDeterminer));
      if (typeEquivalencyDeterminer == null) throw new ArgumentNullException(nameof(typeEquivalencyDeterminer));
      _nameSimilarityDeterminer = nameSimilarityDeterminer;
      _typeEquivalencyDeterminer = typeEquivalencyDeterminer;
    }

    public bool Matches(IProperty source, IProperty target) {
      return _nameSimilarityDeterminer.AreSimilar(source.PropertyDeclarationSyntax.Identifier.Text,
        target.PropertyDeclarationSyntax.Identifier.Text)
             &&
             _typeEquivalencyDeterminer.AreEquivalent(source.PropertyDeclarationSyntax.Type,
               target.PropertyDeclarationSyntax.Type);
    }
  }
}