using System;

namespace AdapterGenerator.Core.Matching {
  public class LevenshteinDistanceCalculator : ILevenshteinDistanceCalculator {
    public int Calculate(string first, string second) {
      if (string.IsNullOrEmpty(first)) {
        if (string.IsNullOrEmpty(second))
          return 0;
        return second.Length;
      }

      if (string.IsNullOrEmpty(second)) {
        return first.Length;
      }

      int n = first.Length;
      int m = second.Length;
      int[,] d = new int[n + 1, m + 1];

      // initialize the top and right of the table to 0, 1, 2, ...
      for (int i = 0; i <= n; i++) {
        d[i, 0] = i;
      }
      for (int j = 1; j <= m; d[0, j] = j++) {
        d[0, j] = j;
      }

      for (int i = 1; i <= n; i++) {
        for (int j = 1; j <= m; j++) {
          int cost = (second[j - 1] == first[i - 1]) ? 0 : 1;
          int min1 = d[i - 1, j] + 1;
          int min2 = d[i, j - 1] + 1;
          int min3 = d[i - 1, j - 1] + cost;
          d[i, j] = Math.Min(Math.Min(min1, min2), min3);
        }
      }
      return d[n, m];
    }
  }
}