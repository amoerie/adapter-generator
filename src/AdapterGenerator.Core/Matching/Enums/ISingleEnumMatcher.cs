using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public interface ISingleEnumMatcher {
    /// <summary>
    /// Tries to find a match for the specified source in the specified targets
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="targets">The targets.</param>
    IEnumMatch Match(IEnum source, IImmutableList<IEnum> targets);
  }
}