using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public interface IClassMatchWithoutPropertyMatches {
    IClass Source { get; set; }
    IClass Target { get; set; }
  }

  public class ClassMatchWithoutPropertyMatches : IClassMatchWithoutPropertyMatches {
    public IClass Source { get; set; }
    public IClass Target { get; set; }
  }
}