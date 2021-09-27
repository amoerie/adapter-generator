using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching.Classes {
  public interface IClassMatch {
    IClass Source { get; }
    IClass Target { get; }
    IImmutableList<IPropertyMatch> PropertyMatches { get; }
  }
}