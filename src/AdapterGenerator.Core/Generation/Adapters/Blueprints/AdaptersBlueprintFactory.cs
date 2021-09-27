using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq;
using AdapterGenerator.Core.Generation.Adapters.Properties;

namespace AdapterGenerator.Core.Generation.Adapters.Blueprints {
  public class AdaptersBlueprintFactory : IAdaptersBlueprintFactory {
    private readonly ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;
    private readonly ILogger _logger;

    public AdaptersBlueprintFactory(ITypeSyntaxAnalyzer typeSyntaxAnalyzer, ILogger logger) {
      if (typeSyntaxAnalyzer == null) throw new ArgumentNullException(nameof(typeSyntaxAnalyzer));
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      _typeSyntaxAnalyzer = typeSyntaxAnalyzer;
      _logger = logger;
    }

    public IImmutableList<IAdapterBlueprint> CreateBlueprints(IMatches matches) {
      if (matches == null) throw new ArgumentNullException(nameof(matches));
      _logger.Info("Generating adapter blueprints");
      return CreateBluePrintsInternal(matches).ToImmutableList();
    }

    private IEnumerable<IAdapterBlueprint> CreateBluePrintsInternal(IMatches matches) {
      foreach (var classMatch in matches.ClassMatches) {
        yield return CreateBlueprint(matches, classMatch);
      }
      foreach (var enumMatch in matches.EnumMatches) {
        yield return CreateBlueprint(enumMatch);
      }
    }

    private IAdapterBlueprint CreateBlueprint(IMatches matches, IClassMatch classMatch) {
      Func<IdentifierNameSyntax, IAdapterBlueprint> createByClassIdentifier =
        identifierType => identifierType != null
          ? matches.ClassMatches
            .Where(c => string.Equals(c.Target.ClassDeclarationSyntax.Identifier.Text, identifierType.Identifier.Text))
            .Select(c => CreateBlueprint(matches, c))
            .FirstOrDefault()
          : null;
      Func<IdentifierNameSyntax, IAdapterBlueprint> createByEnumIdentifier =
        identifierType => identifierType != null
          ? matches.EnumMatches
            .Where(c => string.Equals(c.Target.EnumDeclarationSyntax.Identifier.Text, identifierType.Identifier.Text))
            .Select(CreateBlueprint)
            .FirstOrDefault()
          : null;
      Func<IdentifierNameSyntax, IAdapterBlueprint> createByIdentifier =
        identifierType => createByClassIdentifier(identifierType) ?? createByEnumIdentifier(identifierType);
      Func<GenericNameSyntax, IAdapterBlueprint> createByEnumerableType =
        genericType => genericType != null && _typeSyntaxAnalyzer.ImplementsIEnumerable(genericType)
          ? createByIdentifier(
            _typeSyntaxAnalyzer.ExtractElementTypeFromEnumerable(genericType) as IdentifierNameSyntax)
          : null;
      Func<ArrayTypeSyntax, IAdapterBlueprint> createByArrayType =
        arrayType => arrayType != null
          ? createByIdentifier(arrayType.ElementType as IdentifierNameSyntax)
          : null;
      Func<IPropertyMatch, IAdapterBlueprint> createNestedAdapterBlueprint =
        propertyMatch =>
          createByIdentifier(propertyMatch.Target.PropertyDeclarationSyntax.Type as IdentifierNameSyntax)
          ?? createByArrayType(propertyMatch.Target.PropertyDeclarationSyntax.Type as ArrayTypeSyntax)
          ?? createByEnumerableType(propertyMatch.Target.PropertyDeclarationSyntax.Type as GenericNameSyntax);
      var nestedAdapterBlueprints = classMatch.PropertyMatches
        .Select(createNestedAdapterBlueprint)
        .Where(c => c != null)
        .DistinctBy(c => c.QualifiedName.ToFullString())
        .ToImmutableList();
      return new ClassAdapterBlueprint(classMatch, nestedAdapterBlueprints);
    }

    private IAdapterBlueprint CreateBlueprint(IEnumMatch enumMatch) {
      return new EnumAdapterBlueprint(enumMatch);
    }
  }
}