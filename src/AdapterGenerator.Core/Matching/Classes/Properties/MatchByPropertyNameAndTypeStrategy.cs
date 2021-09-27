using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Core.Matching.Enums;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class MatchByPropertyNameAndTypeStrategy : IPropertyMatchingStrategy {
    private readonly ITypeEquivalencyDeterminer _typeEquivalencyDeterminer;

    public MatchByPropertyNameAndTypeStrategy(ITypeEquivalencyDeterminer typeEquivalencyDeterminer) {
      if (typeEquivalencyDeterminer == null) throw new ArgumentNullException(nameof(typeEquivalencyDeterminer));
      _typeEquivalencyDeterminer = typeEquivalencyDeterminer;
    }

    public bool Matches(IProperty source, IProperty target) {
      return string.Equals(source.PropertyDeclarationSyntax.Identifier.Text,
        target.PropertyDeclarationSyntax.Identifier.Text, StringComparison.OrdinalIgnoreCase)
             &&
             _typeEquivalencyDeterminer.AreEquivalent(source.PropertyDeclarationSyntax.Type,
               target.PropertyDeclarationSyntax.Type);
    }
  }
}