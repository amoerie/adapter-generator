using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class PropertyMatcher : IPropertyMatcher {
    private readonly ISinglePropertyMatcher _singlePropertyMatcher;

    public PropertyMatcher(ISinglePropertyMatcher singlePropertyMatcher) {
      if (singlePropertyMatcher == null) throw new ArgumentNullException(nameof(singlePropertyMatcher));
      _singlePropertyMatcher = singlePropertyMatcher;
    }

    public IImmutableList<IPropertyMatch> Match(IImmutableList<IClassMatchWithoutPropertyMatches> classMatches,
      IImmutableList<IEnumMatch> enumMatches, IClass source, IClass target) {
      return source.Properties
        .Select(property => _singlePropertyMatcher.Match(classMatches, enumMatches, property, target.Properties))
        .Where(match => match != null)
        .ToImmutableList();
    }
  }
}