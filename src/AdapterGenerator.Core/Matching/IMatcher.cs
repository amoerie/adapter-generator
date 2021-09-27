using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching {
  public interface IMatcher {
    IMatches Match(ITypeDeclarations sources, ITypeDeclarations targets);
  }
}