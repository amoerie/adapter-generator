using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public interface IPropertyMatcher {
    /// <summary>
    /// Matches the properties specified source class with the properties of the specified target class.
    /// </summary>
    IImmutableList<IPropertyMatch> Match(IImmutableList<IClassMatchWithoutPropertyMatches> classMatches,
      IImmutableList<IEnumMatch> enumMatches, IClass source, IClass target);
  }
}