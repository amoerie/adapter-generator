using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Enums;

namespace AdapterGenerator.Core.Matching {
  public class Matches : IMatches {
    public IImmutableList<IClassMatch> ClassMatches { get; }
    public IImmutableList<IEnumMatch> EnumMatches { get; }

    public Matches(IImmutableList<IClassMatch> classMatches, IImmutableList<IEnumMatch> enumMatches) {
      if (classMatches == null) throw new ArgumentNullException(nameof(classMatches));
      if (enumMatches == null) throw new ArgumentNullException(nameof(enumMatches));
      ClassMatches = classMatches;
      EnumMatches = enumMatches;
    }
  }
}