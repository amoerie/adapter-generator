using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public class EnumMatcher : IEnumMatcher {
    private readonly ISingleEnumMatcher _singleEnumMatcher;

    public EnumMatcher(ISingleEnumMatcher singleEnumMatcher) {
      if (singleEnumMatcher == null) throw new ArgumentNullException(nameof(singleEnumMatcher));
      _singleEnumMatcher = singleEnumMatcher;
    }

    public IImmutableList<IEnumMatch> Match(IImmutableList<IEnum> sources, IImmutableList<IEnum> targets) {
      return sources
        .Select(source => _singleEnumMatcher.Match(source, targets))
        .Where(match => match != null)
        .ToImmutableList();
    }
  }
}