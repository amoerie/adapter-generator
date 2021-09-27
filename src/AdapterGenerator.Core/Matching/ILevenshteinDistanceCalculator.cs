namespace AdapterGenerator.Core.Matching {
  public interface ILevenshteinDistanceCalculator {
    int Calculate(string first, string second);
  }
}