using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public interface ISingleClassMatcher {
    /// <summary>
    /// Tries to find a match for the specified source in the specified targets
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="targets">The targets.</param>
    IClassMatchWithoutPropertyMatches Match(IClass source, IImmutableList<IClass> targets);
  }
}