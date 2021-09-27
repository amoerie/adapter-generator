using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.AdapterTests;

namespace AdapterGenerator.Core.Generation {
  public interface IPropertyMatchAnalyzerFactory {
    IPropertyMatchAnalyzer Create(IClassAdapterGenerationContext context);
    IPropertyMatchAnalyzer Create(IClassAdapterTestsGenerationContext context);
  }
}