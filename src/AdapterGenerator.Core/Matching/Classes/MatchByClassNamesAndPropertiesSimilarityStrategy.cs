using System;
using System.Linq;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public class MatchByClassNamesAndPropertiesSimilarityStrategy : IClassMatchingStrategy {
    private readonly INameSimilarityDeterminer _nameSimilarityDeterminer;

    public MatchByClassNamesAndPropertiesSimilarityStrategy(INameSimilarityDeterminer nameSimilarityDeterminer) {
      if (nameSimilarityDeterminer == null) throw new ArgumentNullException(nameof(nameSimilarityDeterminer));
      _nameSimilarityDeterminer = nameSimilarityDeterminer;
    }

    public bool Matches(IClass source, IClass target) {
      string sourceName = source.ClassDeclarationSyntax.Identifier.Text;
      string targetName = target.ClassDeclarationSyntax.Identifier.Text;
      double numberOfSimilarProperties = target.Properties
        .Count(targetProperty => source.Properties.Any(sourceProperty =>
          _nameSimilarityDeterminer.AreSimilar(targetProperty.PropertyDeclarationSyntax.Identifier.Text,
            sourceProperty.PropertyDeclarationSyntax.Identifier.Text)));
      return _nameSimilarityDeterminer.AreSimilar(sourceName, targetName) // names are similar
             && numberOfSimilarProperties/target.Properties.Count > 0.5;
        // and more than half of target properties are similarly named
    }
  }
}