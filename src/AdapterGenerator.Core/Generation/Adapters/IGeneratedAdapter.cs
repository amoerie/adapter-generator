using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IGeneratedAdapter {
    IAdapterBlueprint Blueprint { get; }
    CompilationUnitSyntax CompilationUnitSyntax { get; }
  }

  public interface IGeneratedClassAdapter : IGeneratedAdapter {
    new IClassAdapterBlueprint Blueprint { get; }
  }

  public interface IGeneratedEnumAdapter : IGeneratedAdapter {
    new IEnumAdapterBlueprint Blueprint { get; }
  }
}