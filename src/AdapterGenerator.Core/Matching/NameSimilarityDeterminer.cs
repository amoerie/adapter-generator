using System;

namespace AdapterGenerator.Core.Matching {
  public class NameSimilarityDeterminer : INameSimilarityDeterminer {
    private readonly ILevenshteinDistanceCalculator _levenshteinDistanceCalculator;

    public NameSimilarityDeterminer(ILevenshteinDistanceCalculator levenshteinDistanceCalculator) {
      if (levenshteinDistanceCalculator == null)
        throw new ArgumentNullException(nameof(levenshteinDistanceCalculator));
      _levenshteinDistanceCalculator = levenshteinDistanceCalculator;
    }

    public bool AreSimilar(string name, string otherName) {
      return string.Equals(name, otherName, StringComparison.OrdinalIgnoreCase)
             || name.IndexOf(otherName, StringComparison.OrdinalIgnoreCase) > -1
             || otherName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1
             || _levenshteinDistanceCalculator.Calculate(name, otherName) < 3; // less than 3 changes necessary
    }
  }
}