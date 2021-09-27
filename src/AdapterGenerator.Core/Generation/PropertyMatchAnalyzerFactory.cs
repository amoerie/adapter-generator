using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.AdapterTests;

namespace AdapterGenerator.Core.Generation {
  public class PropertyMatchAnalyzerFactory : IPropertyMatchAnalyzerFactory {
    public IPropertyMatchAnalyzer Create(IClassAdapterGenerationContext context) {
      return new PropertyMatchAnalyzer(context.Blueprint,
        new TypeSyntaxComparer(context.Sources, context.Targets),
        new TypeSyntaxAnalyzer());
    }

    public IPropertyMatchAnalyzer Create(IClassAdapterTestsGenerationContext context) {
      return new PropertyMatchAnalyzer(context.Adapter.Blueprint,
        new TypeSyntaxComparer(context.Sources, context.Targets),
        new TypeSyntaxAnalyzer());
    }
  }
}