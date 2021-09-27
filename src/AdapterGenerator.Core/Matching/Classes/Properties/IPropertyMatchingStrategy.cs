using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public interface IPropertyMatchingStrategy {
    bool Matches(IProperty source, IProperty target);
  }
}