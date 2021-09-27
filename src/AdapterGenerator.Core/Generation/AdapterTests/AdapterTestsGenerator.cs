using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public class AdapterTestsGenerator : IAdapterTestsGenerator {
    private readonly IAdapterTestsCompilationUnitGenerator _compilationUnitGenerator;
    private readonly ILogger _logger;

    public AdapterTestsGenerator(IAdapterTestsCompilationUnitGenerator compilationUnitGenerator, ILogger logger) {
      if (compilationUnitGenerator == null)
        throw new ArgumentNullException(nameof(compilationUnitGenerator));
      _compilationUnitGenerator = compilationUnitGenerator;
      _logger = logger;
    }

    public IImmutableList<IGeneratedAdapterTests> Generate(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> adapters) {
      Func<IGeneratedClassAdapter, IGeneratedAdapterTests> createClassAdapterTests = classAdapter => {
        _logger.Info("Generating unit tests for " + classAdapter.Blueprint.Name.Identifier.Text);
        var generationContext = ClassAdapterTestsGenerationContext.Create(sources, targets, adapters, classAdapter);
        var compilationUnit = _compilationUnitGenerator.Generate(generationContext);
        return new GeneratedAdapterTests(generationContext.TestClassName, classAdapter, compilationUnit);
      };

      Func<IGeneratedEnumAdapter, IGeneratedAdapterTests> createEnumAdapterTests = enumAdapter => {
        _logger.Info("Generating unit tests for " + enumAdapter.Blueprint.Name.Identifier.Text);
        var generationContext = EnumAdapterTestsGenerationContext.Create(sources, targets, adapters, enumAdapter);
        var compilationUnit = _compilationUnitGenerator.Generate(generationContext);
        return new GeneratedAdapterTests(generationContext.TestClassName, enumAdapter, compilationUnit);
      };

      var adapterTests = adapters.OfType<IGeneratedClassAdapter>().Select(createClassAdapterTests)
        .Concat(adapters.OfType<IGeneratedEnumAdapter>().Select(createEnumAdapterTests))
        .ToImmutableList();
      return adapterTests;
    }
  }
}