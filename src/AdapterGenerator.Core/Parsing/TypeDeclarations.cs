using System.Collections.Immutable;

namespace AdapterGenerator.Core.Parsing {
  public class TypeDeclarations : ITypeDeclarations {
    public IImmutableList<IClass> Classes { get; set; }
    public IImmutableList<IEnum> Enums { get; set; }

    public ITypeDeclarations Union(ITypeDeclarations typeDeclarations) {
      return new TypeDeclarations {
        Classes = Classes.AddRange(typeDeclarations.Classes),
        Enums = Enums.AddRange(typeDeclarations.Enums)
      };
    }
  }
}