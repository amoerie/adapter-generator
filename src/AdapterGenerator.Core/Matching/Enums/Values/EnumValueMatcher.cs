using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public class EnumValueMatcher : IEnumValueMatcher {
    private readonly ISingleEnumValueMatcher _singleEnumValueMatcher;

    public EnumValueMatcher(ISingleEnumValueMatcher singleEnumValueMatcher) {
      if (singleEnumValueMatcher == null) throw new ArgumentNullException(nameof(singleEnumValueMatcher));
      _singleEnumValueMatcher = singleEnumValueMatcher;
    }

    public IImmutableList<IEnumValueMatch> Match(IEnum source, IEnum target) {
      return source.Values
        .Select(enumValue => _singleEnumValueMatcher.Match(enumValue, target.Values))
        .Where(match => match != null)
        .ToImmutableList();
    }
  }
}