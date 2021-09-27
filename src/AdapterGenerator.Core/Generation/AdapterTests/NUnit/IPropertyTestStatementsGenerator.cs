using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IPropertyTestStatementsGenerator {
    IImmutableList<StatementSyntax> Generate();
  }
}