using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public class ClassMatch : IClassMatch {
    public IClass Source { get; set; }
    public IClass Target { get; set; }
    public IImmutableList<IPropertyMatch> PropertyMatches { get; set; }
  }
}