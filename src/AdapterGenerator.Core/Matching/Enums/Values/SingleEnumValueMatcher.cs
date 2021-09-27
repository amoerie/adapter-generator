using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Enums.Values {
  public class SingleEnumValueMatcher : ISingleEnumValueMatcher {
    private readonly ILogger _logger;
    private readonly IEnumValueMatchingStrategy[] _enumValueMatchingStrategies;

    public SingleEnumValueMatcher(ILogger logger, IEnumerable<IEnumValueMatchingStrategy> matchingStrategies) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      if (matchingStrategies == null) throw new ArgumentNullException(nameof(matchingStrategies));
      _logger = logger;
      _enumValueMatchingStrategies = matchingStrategies.ToArray();
    }

    public IEnumValueMatch Match(IEnumValue source, IImmutableList<IEnumValue> targets) {
      var match = _enumValueMatchingStrategies
        .Select(strategy => targets.RemoveAll(target => !strategy.Matches(source, target)))
        .Where(matches => matches.Count == 1)
        .Select(matchingTargets => matchingTargets.SingleOrDefault())
        .Select(matchingTarget => new EnumValueMatch {Source = source, Target = matchingTarget})
        .FirstOrDefault();
      if (match == null)
        _logger.Error(
          $"No single match could be determined for {source.Enum.QualifiedName.ToFullString()}.{source.EnumMemberDeclarationSyntax.Identifier.Text}");
      else
        _logger.Info(
          $"Matched enum value {match.Source.Enum.EnumDeclarationSyntax.Identifier.Text}.{match.Source.EnumMemberDeclarationSyntax.Identifier.Text}" +
          $" to {match.Target.Enum.EnumDeclarationSyntax.Identifier.Text}.{match.Target.EnumMemberDeclarationSyntax.Identifier}");
      return match;
    }
  }
}