using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class SinglePropertyMatcher : ISinglePropertyMatcher {
    private readonly ILogger _logger;
    private readonly IPropertyMatchingStrategiesFactory _propertyMatchingStrategiesFactory;

    public SinglePropertyMatcher(ILogger logger, IPropertyMatchingStrategiesFactory propertyMatchingStrategiesFactory) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      if (propertyMatchingStrategiesFactory == null)
        throw new ArgumentNullException(nameof(propertyMatchingStrategiesFactory));
      _logger = logger;
      _propertyMatchingStrategiesFactory = propertyMatchingStrategiesFactory;
    }

    public IPropertyMatch Match(IImmutableList<IClassMatchWithoutPropertyMatches> classMatches,
      IImmutableList<IEnumMatch> enumMatches, IProperty source, IImmutableList<IProperty> targets) {
      var match = _propertyMatchingStrategiesFactory.Create(classMatches, enumMatches)
        .Select(strategy => targets.RemoveAll(target => !strategy.Matches(source, target)))
        .Where(matches => matches.Count == 1)
        .Select(matchingTargets => matchingTargets.SingleOrDefault())
        .Select(matchingTarget => new PropertyMatch {Source = source, Target = matchingTarget})
        .FirstOrDefault();
      if (match == null)
        _logger.Error(
          $"No single match could be determined for {source.Class.QualifiedName.ToFullString()}.{source.PropertyDeclarationSyntax.Identifier.Text}");
      else
        _logger.Info(
          $"Matched property {match.Source.Class.ClassDeclarationSyntax.Identifier.Text}.{match.Source.PropertyDeclarationSyntax.Identifier.Text}" +
          $" to {match.Target.Class.ClassDeclarationSyntax.Identifier.Text}.{match.Target.PropertyDeclarationSyntax.Identifier}");
      return match;
    }
  }
}