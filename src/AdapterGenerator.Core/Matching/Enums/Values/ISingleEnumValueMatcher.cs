using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public interface ISingleEnumValueMatcher {
    IEnumValueMatch Match(IEnumValue source, IImmutableList<IEnumValue> targets);
  }
}