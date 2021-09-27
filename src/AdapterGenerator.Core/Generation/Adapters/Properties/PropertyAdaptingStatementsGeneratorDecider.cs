using System;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public class PropertyAdaptingStatementsGeneratorDecider : IPropertyAdaptingStatementsGeneratorDecider {
    private readonly IPropertyMatchAnalyzerFactory _propertyMatchAnalyzerFactory;
    private readonly ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;

    public PropertyAdaptingStatementsGeneratorDecider(IPropertyMatchAnalyzerFactory propertyMatchAnalyzerFactory,
      ITypeSyntaxAnalyzer typeSyntaxAnalyzer) {
      if (propertyMatchAnalyzerFactory == null) throw new ArgumentNullException(nameof(propertyMatchAnalyzerFactory));
      if (typeSyntaxAnalyzer == null) throw new ArgumentNullException(nameof(typeSyntaxAnalyzer));
      _propertyMatchAnalyzerFactory = propertyMatchAnalyzerFactory;
      _typeSyntaxAnalyzer = typeSyntaxAnalyzer;
    }

    public IPropertyAdaptingStatementsGenerator Decide(
      IClassAdapterGenerationContextWithClass classAdapterGenerationContext, IProperty targetProperty) {
      var propertyMatch =
        classAdapterGenerationContext.Blueprint.ClassMatch.PropertyMatches.FirstOrDefault(
          p => p.Target == targetProperty);
      if (propertyMatch == null)
        return new AssignDefaultValueStatementsGenerator(classAdapterGenerationContext, targetProperty);
      var propertyMatchAnalyzer = _propertyMatchAnalyzerFactory.Create(classAdapterGenerationContext);
      if (propertyMatchAnalyzer.CanBeAdaptedThroughSimpleAssignment(propertyMatch))
        return new SimpleAssignmentStatementsGenerator(classAdapterGenerationContext, propertyMatch);
      IAdapterBlueprint nestedAdapter;
      if ((nestedAdapter = propertyMatchAnalyzer.FindNestedAdapterForSimpleAssignment(propertyMatch)) != null)
        return new NestedAdapterStatementsGenerator(classAdapterGenerationContext, propertyMatch, nestedAdapter);
      if ((nestedAdapter = propertyMatchAnalyzer.FindNestedAdapterForEnumerableAssigmnent(propertyMatch)) != null)
        return new NestedAdapterForIEnumerableStatementsGenerator(classAdapterGenerationContext, propertyMatch,
          nestedAdapter, _typeSyntaxAnalyzer);
      return new AssignDefaultValueStatementsGenerator(classAdapterGenerationContext, targetProperty);
    }
  }
}