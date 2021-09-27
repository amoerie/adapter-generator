using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public interface IPropertySpecificStatementsGenerator {
    IImmutableList<StatementSyntax> Generate(IClassAdapterGenerationContextWithClass classAdapterGenerationContext,
      IProperty targetProperty);
  }
}