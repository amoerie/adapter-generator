using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public class PropertySpecificStatementsGenerator : IPropertySpecificStatementsGenerator {
    private readonly IPropertyAdaptingStatementsGeneratorDecider _decider;

    public PropertySpecificStatementsGenerator(IPropertyAdaptingStatementsGeneratorDecider decider) {
      if (decider == null) throw new ArgumentNullException(nameof(decider));
      _decider = decider;
    }

    public IImmutableList<StatementSyntax> Generate(
      IClassAdapterGenerationContextWithClass classAdapterGenerationContext, IProperty targetProperty) {
      var generator = _decider.Decide(classAdapterGenerationContext, targetProperty);
      return generator.Generate();
    }
  }
}