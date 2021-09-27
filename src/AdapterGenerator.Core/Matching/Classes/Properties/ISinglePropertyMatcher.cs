using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public interface ISinglePropertyMatcher {
    /// <summary>
    /// Matches a single property against a collection of targets, returning the best match or null if no single match could be determined
    /// </summary>
    IPropertyMatch Match(IImmutableList<IClassMatchWithoutPropertyMatches> classMatches,
      IImmutableList<IEnumMatch> enumMatches, IProperty source, IImmutableList<IProperty> targets);
  }
}