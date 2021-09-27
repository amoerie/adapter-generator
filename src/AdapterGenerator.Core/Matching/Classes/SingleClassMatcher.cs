using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Core.Matching.Classes.Properties;

namespace AdapterGenerator.Core.Matching.Classes {
  public class SingleClassMatcher : ISingleClassMatcher {
    private readonly ILogger _logger;
    private readonly IClassMatchingStrategy[] _classMatchingStrategies;

    public SingleClassMatcher(ILogger logger, IEnumerable<IClassMatchingStrategy> matchingStrategies) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      if (matchingStrategies == null) throw new ArgumentNullException(nameof(matchingStrategies));
      _logger = logger;
      _classMatchingStrategies = matchingStrategies.ToArray();
    }

    public IClassMatchWithoutPropertyMatches Match(IClass source, IImmutableList<IClass> targets) {
      // find and apply a strategy that returns exactly 1 match
      var match = _classMatchingStrategies
        .Select(strategy => targets.RemoveAll(target => !strategy.Matches(source, target)))
        .Where(matches => matches.Count == 1)
        .Select(matchingTargets => matchingTargets.Single())
        .Select(target => new ClassMatchWithoutPropertyMatches {
          Source = source,
          Target = target
        })
        .FirstOrDefault();
      if (match == null)
        _logger.Error($"No single match could be determined for {source.QualifiedName.ToFullString()}");
      else
        _logger.Info(
          $"Matched class {match.Source.ClassDeclarationSyntax.Identifier.Text} to {match.Target.ClassDeclarationSyntax.Identifier.Text}");
      return match;
    }
  }
}