using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public interface IPropertyMatch {
    IProperty Source { get; set; }
    IProperty Target { get; set; }
  }
}