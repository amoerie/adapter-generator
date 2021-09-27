using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public interface IPropertyAdaptingStatementsGenerator {
    IImmutableList<StatementSyntax> Generate();
  }
}