using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IPropertyTestStatementsGeneratorFactory {
    IPropertyTestStatementsGenerator Create(IClassAdapterTestsGenerationContextWithClass context,
      IProperty targetProperty);
  }
}