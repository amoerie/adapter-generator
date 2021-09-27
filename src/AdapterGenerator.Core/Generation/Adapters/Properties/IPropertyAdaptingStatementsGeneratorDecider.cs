using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public interface IPropertyAdaptingStatementsGeneratorDecider {
    IPropertyAdaptingStatementsGenerator Decide(
      IClassAdapterGenerationContextWithClass classAdapterGenerationContextWithClass, IProperty targetProperty);
  }
}