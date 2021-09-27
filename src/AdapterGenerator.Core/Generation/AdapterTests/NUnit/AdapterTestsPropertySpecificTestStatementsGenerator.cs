using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsPropertySpecificTestStatementsGenerator :
    IAdapterTestsPropertySpecificTestStatementsGenerator {
    private readonly IPropertyTestStatementsGeneratorFactory _propertyTestStatementsGeneratorFactory;

    public AdapterTestsPropertySpecificTestStatementsGenerator(
      IPropertyTestStatementsGeneratorFactory propertyTestStatementsGeneratorFactory) {
      if (propertyTestStatementsGeneratorFactory == null)
        throw new ArgumentNullException(nameof(propertyTestStatementsGeneratorFactory));
      _propertyTestStatementsGeneratorFactory = propertyTestStatementsGeneratorFactory;
    }

    public IImmutableList<StatementSyntax> Generate(IClassAdapterTestsGenerationContextWithClass context,
      IProperty targetProperty) {
      var generator = _propertyTestStatementsGeneratorFactory.Create(context, targetProperty);
      return generator.Generate();
    }
  }
}