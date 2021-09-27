using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums {
  public class SingleEnumMatcher : ISingleEnumMatcher {
    private readonly ILogger _logger;
    private readonly IEnumValueMatcher _enumValueMatcher;
    private readonly IEnumMatchingStrategy[] _enumMatchingStrategies;

    public SingleEnumMatcher(ILogger logger, IEnumerable<IEnumMatchingStrategy> matchingStrategies,
      IEnumValueMatcher enumValueMatcher) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      if (matchingStrategies == null) throw new ArgumentNullException(nameof(matchingStrategies));
      if (enumValueMatcher == null) throw new ArgumentNullException(nameof(enumValueMatcher));
      _logger = logger;
      _enumValueMatcher = enumValueMatcher;
      _enumMatchingStrategies = matchingStrategies.ToArray();
    }

    public IEnumMatch Match(IEnum source, IImmutableList<IEnum> targets) {
      // find and apply a strategy that returns exactly 1 match
      var match = _enumMatchingStrategies
        .Select(strategy => targets.RemoveAll(target => !strategy.Matches(source, target)))
        .Where(matches => matches.Count == 1)
        .Select(matchingTargets => matchingTargets.Single())
        .Select(target => new EnumMatch {
          Source = source,
          Target = target,
          ValueMatches = _enumValueMatcher.Match(source, target)
        })
        .FirstOrDefault();
      if (match == null)
        _logger.Error($"No single match could be determined for {source.QualifiedName.ToFullString()}");
      else
        _logger.Info(
          $"Matched enum {match.Source.EnumDeclarationSyntax.Identifier.Text} to {match.Target.EnumDeclarationSyntax.Identifier.Text}");
      return match;
    }
  }
}