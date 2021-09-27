using System;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class PropertyTestStatementsGeneratorFactory : IPropertyTestStatementsGeneratorFactory {
    private readonly IPropertyMatchAnalyzerFactory _propertyMatchAnalyzerFactory;
    private readonly IDummyValueFactory _dummyValueFactory;
    private readonly ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;

    public PropertyTestStatementsGeneratorFactory(IPropertyMatchAnalyzerFactory propertyMatchAnalyzerFactory,
      IDummyValueFactory dummyValueFactory, ITypeSyntaxAnalyzer typeSyntaxAnalyzer) {
      if (propertyMatchAnalyzerFactory == null) throw new ArgumentNullException(nameof(propertyMatchAnalyzerFactory));
      if (dummyValueFactory == null) throw new ArgumentNullException(nameof(dummyValueFactory));
      if (typeSyntaxAnalyzer == null) throw new ArgumentNullException(nameof(typeSyntaxAnalyzer));
      _propertyMatchAnalyzerFactory = propertyMatchAnalyzerFactory;
      _dummyValueFactory = dummyValueFactory;
      _typeSyntaxAnalyzer = typeSyntaxAnalyzer;
    }

    public IPropertyTestStatementsGenerator Create(IClassAdapterTestsGenerationContextWithClass context,
      IProperty targetProperty) {
      var propertyMatch =
        context.Adapter.Blueprint.ClassMatch.PropertyMatches.FirstOrDefault(pm => pm.Target == targetProperty);
      if (propertyMatch == null)
        return new PropertyTestStatementsForDefaultValueGenerator(context, targetProperty);
      var propertyMatchAnalyzer = _propertyMatchAnalyzerFactory.Create(context);
      if (propertyMatchAnalyzer.CanBeAdaptedThroughSimpleAssignment(propertyMatch)) {
        return new PropertyTestStatementsForSimpleAssignmentGenerator(context, propertyMatch, _dummyValueFactory);
      }
      IAdapterBlueprint nestedAdapterBlueprint;
      if ((nestedAdapterBlueprint = propertyMatchAnalyzer.FindNestedAdapterForSimpleAssignment(propertyMatch)) != null) {
        return new PropertyTestStatementsForSingleValueWithNestedAdapterGenerator(context, propertyMatch,
          _dummyValueFactory, nestedAdapterBlueprint);
      }
      if ((nestedAdapterBlueprint = propertyMatchAnalyzer.FindNestedAdapterForEnumerableAssigmnent(propertyMatch)) !=
          null) {
        return new PropertyTestStatementsForEnumerableWithNestedAdapterGenerator(context, propertyMatch,
          _dummyValueFactory, nestedAdapterBlueprint);
      }
      // if all else fails, return default value
      return new PropertyTestStatementsForDefaultValueGenerator(context, targetProperty);
    }
  }
}