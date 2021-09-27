using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public class ClassMatcher : IClassMatcher {
    private readonly ISingleClassMatcher _singleClassMatcher;

    public ClassMatcher(ISingleClassMatcher singleClassMatcher) {
      if (singleClassMatcher == null) throw new ArgumentNullException(nameof(singleClassMatcher));
      _singleClassMatcher = singleClassMatcher;
    }

    public IImmutableList<IClassMatchWithoutPropertyMatches> Match(IImmutableList<IClass> sources,
      IImmutableList<IClass> targets) {
      return sources
        .Select(source => _singleClassMatcher.Match(source, targets))
        .Where(match => match != null)
        .ToImmutableList();
    }
  }
}