using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public interface IEnumValueMatcher {
    IImmutableList<IEnumValueMatch> Match(IEnum source, IEnum target);
  }
}