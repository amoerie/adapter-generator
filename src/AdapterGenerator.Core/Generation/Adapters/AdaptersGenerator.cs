using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdaptersGenerator : IAdaptersGenerator {
    private readonly IAdaptersBlueprintFactory _adaptersBlueprintFactory;
    private readonly IAdapterCompilationUnitGenerator _compilationUnitGenerator;
    private readonly ILogger _logger;

    public AdaptersGenerator(IAdaptersBlueprintFactory adaptersBlueprintFactory,
      IAdapterCompilationUnitGenerator compilationUnitGenerator,
      ILogger logger) {
      if (adaptersBlueprintFactory == null) throw new ArgumentNullException(nameof(adaptersBlueprintFactory));
      if (compilationUnitGenerator == null) throw new ArgumentNullException(nameof(compilationUnitGenerator));
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      _adaptersBlueprintFactory = adaptersBlueprintFactory;
      _compilationUnitGenerator = compilationUnitGenerator;
      _logger = logger;
    }

    public IImmutableList<IGeneratedAdapter> Generate(ITypeDeclarations sources, ITypeDeclarations targets,
      IMatches matches) {
      var blueprints = _adaptersBlueprintFactory.CreateBlueprints(matches);

      Func<IClassAdapterBlueprint, IGeneratedAdapter> createClassAdapter = blueprint => {
        _logger.Info("Generating " + blueprint.Name.Identifier.Text);
        var generationContext = ClassAdapterGenerationContext.Create(sources, targets, blueprints, blueprint);
        var compilationUnit = _compilationUnitGenerator.Generate(generationContext);
        return new GeneratedClassAdapter(blueprint, compilationUnit);
      };
      Func<IEnumAdapterBlueprint, IGeneratedAdapter> createEnumAdapter = blueprint => {
        _logger.Info("Generating " + blueprint.Name.Identifier.Text);
        var generationContext = EnumAdapterGenerationContext.Create(sources, targets, blueprints, blueprint);
        var compilationUnit = _compilationUnitGenerator.Generate(generationContext);
        return new GeneratedEnumAdapter(blueprint, compilationUnit);
      };

      var adapters = blueprints.OfType<IClassAdapterBlueprint>().Select(createClassAdapter)
        .Concat(blueprints.OfType<IEnumAdapterBlueprint>().Select(createEnumAdapter))
        .ToImmutableList();
      return adapters;
    }
  }
}