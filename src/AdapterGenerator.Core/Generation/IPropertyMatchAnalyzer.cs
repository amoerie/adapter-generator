using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes.Properties;

namespace AdapterGenerator.Core.Generation {
  public interface IPropertyMatchAnalyzer {
    /// <summary>
    /// Determines whether this instance can be adapted through simple assignment for the specified property match.
    /// </summary>
    bool CanBeAdaptedThroughSimpleAssignment(IPropertyMatch propertyMatch);

    /// <summary>
    /// Determines whether this property match can be adapted through a nested adapter and returns the nested adapter if this is the case
    /// </summary>
    IAdapterBlueprint FindNestedAdapterForSimpleAssignment(IPropertyMatch propertyMatch);

    /// <summary>
    /// Determines whether this property match can be adapted as an enumerable with a nested adapter and returns the nested adapter if this is the case
    /// </summary>
    IAdapterBlueprint FindNestedAdapterForEnumerableAssigmnent(IPropertyMatch propertyMatch);
  }
}