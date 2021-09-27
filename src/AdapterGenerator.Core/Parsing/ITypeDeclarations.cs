using System.Collections.Immutable;

namespace AdapterGenerator.Core.Parsing {
  public interface ITypeDeclarations {
    IImmutableList<IClass> Classes { get; }
    IImmutableList<IEnum> Enums { get; }

    /// <summary>
    /// Combines the current type declarations with the provided declarations
    /// </summary>
    ITypeDeclarations Union(ITypeDeclarations typeDeclarations);
  }
}