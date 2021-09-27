using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public interface IEnumMatchingStrategy {
    bool Matches(IEnum source, IEnum target);
  }
}