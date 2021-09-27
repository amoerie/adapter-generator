using AdapterGenerator.Core.Generation.Adapters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public interface IGeneratedAdapterTests {
    string TestClassName { get; }
    IGeneratedAdapter Adapter { get; }
    CompilationUnitSyntax CompilationUnitSyntax { get; }
  }
}