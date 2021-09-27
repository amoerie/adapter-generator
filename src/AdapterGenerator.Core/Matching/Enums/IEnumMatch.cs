using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public interface IEnumMatch {
    IEnum Source { get; }
    IEnum Target { get; }
    IImmutableList<IEnumValueMatch> ValueMatches { get; }
  }
}