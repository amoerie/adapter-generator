using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public interface IEnumMatcher {
    IImmutableList<IEnumMatch> Match(IImmutableList<IEnum> sources, IImmutableList<IEnum> targets);
  }
}