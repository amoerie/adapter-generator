using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public class EnumMatch : IEnumMatch {
    public IEnum Source { get; set; }
    public IEnum Target { get; set; }
    public IImmutableList<IEnumValueMatch> ValueMatches { get; set; }
  }
}