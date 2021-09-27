using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsPropertySpecificTestStatementsGenerator {
    IImmutableList<StatementSyntax> Generate(IClassAdapterTestsGenerationContextWithClass context,
      IProperty targetProperty);
  }
}