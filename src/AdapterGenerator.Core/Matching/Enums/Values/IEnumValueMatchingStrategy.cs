using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public interface IEnumValueMatchingStrategy {
    bool Matches(IEnumValue source, IEnumValue target);
  }
}