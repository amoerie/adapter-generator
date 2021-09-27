using System;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation {
  public class PropertyMatchAnalyzer : IPropertyMatchAnalyzer {
    private readonly IClassAdapterBlueprint _blueprint;
    private readonly ITypeSyntaxComparer _typeSyntaxComparer;
    private readonly ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;

    public PropertyMatchAnalyzer(IClassAdapterBlueprint blueprint, ITypeSyntaxComparer typeSyntaxComparer,
      ITypeSyntaxAnalyzer typeSyntaxAnalyzer) {
      if (blueprint == null) throw new ArgumentNullException(nameof(blueprint));
      if (typeSyntaxComparer == null) throw new ArgumentNullException(nameof(typeSyntaxComparer));
      if (typeSyntaxAnalyzer == null) throw new ArgumentNullException(nameof(typeSyntaxAnalyzer));
      _blueprint = blueprint;
      _typeSyntaxComparer = typeSyntaxComparer;
      _typeSyntaxAnalyzer = typeSyntaxAnalyzer;
    }

    private TOut CastAndConvert<TIn1, TIn2, TOut>(TIn1 sourceType, TIn2 targetType, Func<TIn1, TIn2, TOut> convert) {
      return sourceType != null && targetType != null ? convert(sourceType, targetType) : default(TOut);
    }

    private IAdapterBlueprint FindNestedAdapter(IdentifierNameSyntax sourceIdentifier,
      IdentifierNameSyntax targetIdentifier) {
      return _blueprint.NestedAdapters.FirstOrDefault(
        c => string.Equals(c.TargetType.Identifier.Text, targetIdentifier.Identifier.Text)
             && string.Equals(c.SourceType.Identifier.Text, sourceIdentifier.Identifier.Text));
    }

    public bool CanBeAdaptedThroughSimpleAssignment(IPropertyMatch propertyMatch) {
      // you can do "target.Prop = source.Prop"
      // when they are exactly the same type
      // or when the source is nullable and the target isn't
      var sourceType = propertyMatch.Source.PropertyDeclarationSyntax.Type;
      var targetType = propertyMatch.Target.PropertyDeclarationSyntax.Type;
      return _typeSyntaxComparer.Equals(sourceType, targetType)
             ||
             CastAndConvert(sourceType, targetType as NullableTypeSyntax,
               (s, t) => _typeSyntaxComparer.Equals(s, t.ElementType));
    }

    public IAdapterBlueprint FindNestedAdapterForSimpleAssignment(IPropertyMatch propertyMatch) {
      // you can do "target.Prop = _propAdapter.Adapt(source.Prop)" 
      // when there is a nested adapter for that type
      return CastAndConvert(
        propertyMatch.Source.PropertyDeclarationSyntax.Type as IdentifierNameSyntax,
        propertyMatch.Target.PropertyDeclarationSyntax.Type as IdentifierNameSyntax, FindNestedAdapter);
    }

    public IAdapterBlueprint FindNestedAdapterForEnumerableAssigmnent(IPropertyMatch propertyMatch) {
      // you can do "target.Props = source.Props.Select(p => _propsAdapter.Adapt(p))"
      // when target.Props is an IEnumerable
      // and when source.Props implements IEnumerable
      // and when the element types correspond with a nested adapter
      var sourceType = propertyMatch.Source.PropertyDeclarationSyntax.Type;
      var targetType = propertyMatch.Target.PropertyDeclarationSyntax.Type;

      // check if target.Props is an IEnumerable
      // check if source.Props implements IEnumerable
      // check if the element type is an identifier pointing to a class which has a nested adapter
      var targetIsAdaptableIEnumerableType = _typeSyntaxAnalyzer.IsIEnumerable(targetType)
                                             || _typeSyntaxAnalyzer.IsArray(targetType)
                                             || _typeSyntaxAnalyzer.IsCollectionType(targetType);
      var sourceImplementsIEnumerable = _typeSyntaxAnalyzer.ImplementsIEnumerable(sourceType);
      var targetElementType = _typeSyntaxAnalyzer.ExtractElementTypeFromEnumerable(sourceType);
      var sourceElementType = _typeSyntaxAnalyzer.ExtractElementTypeFromEnumerable(targetType);
      return targetIsAdaptableIEnumerableType && sourceImplementsIEnumerable
        ? CastAndConvert(targetElementType as IdentifierNameSyntax, sourceElementType as IdentifierNameSyntax,
          FindNestedAdapter)
        : null;
    }
  }
}