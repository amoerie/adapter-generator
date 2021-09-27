using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class PropertyMatch : IPropertyMatch {
    public IProperty Source { get; set; }
    public IProperty Target { get; set; }
  }
}