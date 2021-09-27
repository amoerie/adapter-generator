using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public interface IClassMatcher {
    /// <summary>
    /// Matches classes from the specified sources with classes from the specified targets
    /// </summary>
    /// <param name="sources">The sources.</param>
    /// <param name="targets">The targets.</param>
    IImmutableList<IClassMatchWithoutPropertyMatches> Match(IImmutableList<IClass> sources,
      IImmutableList<IClass> targets);
  }
}