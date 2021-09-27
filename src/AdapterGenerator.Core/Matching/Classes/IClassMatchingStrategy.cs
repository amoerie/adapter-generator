using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public interface IClassMatchingStrategy {
    bool Matches(IClass source, IClass target);
  }
}