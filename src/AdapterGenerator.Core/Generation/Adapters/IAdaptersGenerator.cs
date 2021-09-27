using System.Collections.Immutable;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdaptersGenerator {
    IImmutableList<IGeneratedAdapter> Generate(ITypeDeclarations sources, ITypeDeclarations targets, IMatches matches);
  }
}