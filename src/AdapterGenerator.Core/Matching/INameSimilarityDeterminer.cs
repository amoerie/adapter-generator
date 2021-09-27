namespace AdapterGenerator.Core.Matching {
  public interface INameSimilarityDeterminer {
    bool AreSimilar(string name, string otherName);
  }
}