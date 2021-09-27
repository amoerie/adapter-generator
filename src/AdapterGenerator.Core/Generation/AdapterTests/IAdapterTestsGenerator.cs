using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public interface IAdapterTestsGenerator {
    IImmutableList<IGeneratedAdapterTests> Generate(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> adapters);
  }
}