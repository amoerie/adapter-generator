using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public class EnumValueMatch : IEnumValueMatch {
    public IEnumValue Source { get; set; }
    public IEnumValue Target { get; set; }
  }
}