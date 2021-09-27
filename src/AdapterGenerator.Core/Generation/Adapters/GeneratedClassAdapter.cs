using System;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class GeneratedClassAdapter : IGeneratedClassAdapter {
    public IClassAdapterBlueprint Blueprint { get; }
    public CompilationUnitSyntax CompilationUnitSyntax { get; }

    IAdapterBlueprint IGeneratedAdapter.Blueprint => Blueprint;

    public GeneratedClassAdapter(IClassAdapterBlueprint blueprint, CompilationUnitSyntax compilationUnitSyntax) {
      if (blueprint == null) throw new ArgumentNullException(nameof(blueprint));
      if (compilationUnitSyntax == null) throw new ArgumentNullException(nameof(compilationUnitSyntax));
      Blueprint = blueprint;
      CompilationUnitSyntax = compilationUnitSyntax;
    }
  }
}