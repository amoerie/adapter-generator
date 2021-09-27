using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public interface IEnumValueMatch {
    IEnumValue Source { get; }
    IEnumValue Target { get; }
  }
}