using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Enums;

namespace AdapterGenerator.Core.Matching {
  public interface IMatches {
    IImmutableList<IClassMatch> ClassMatches { get; }
    IImmutableList<IEnumMatch> EnumMatches { get; }
  }
}