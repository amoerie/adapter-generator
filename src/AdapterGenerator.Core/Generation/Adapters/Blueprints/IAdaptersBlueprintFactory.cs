using System.Collections.Immutable;
using AdapterGenerator.Core.Matching;

namespace AdapterGenerator.Core.Generation.Adapters.Blueprints {
  public interface IAdaptersBlueprintFactory {
    IImmutableList<IAdapterBlueprint> CreateBlueprints(IMatches matches);
  }
}