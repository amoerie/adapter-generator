using System;
using System.Linq;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation {
  public class TypeSyntaxComparer : ITypeSyntaxComparer {
    private readonly ITypeDeclarations _sources;
    private readonly ITypeDeclarations _targets;

    public TypeSyntaxComparer(ITypeDeclarations sources, ITypeDeclarations targets) {
      if (sources == null) throw new ArgumentNullException(nameof(sources));
      if (targets == null) throw new ArgumentNullException(nameof(targets));
      _sources = sources;
      _targets = targets;
    }

    private bool IsTarget(SyntaxToken identifier) {
      return _targets.Classes.Any(t => string.Equals(t.ClassDeclarationSyntax.Identifier.Text, identifier.Text))
             || _targets.Enums.Any(t => string.Equals(t.EnumDeclarationSyntax.Identifier.Text, identifier.Text));
    }

    private bool IsSource(SyntaxToken identifier) {
      return _sources.Classes.Any(t => string.Equals(t.ClassDeclarationSyntax.Identifier.Text, identifier.Text))
             || _sources.Enums.Any(t => string.Equals(t.EnumDeclarationSyntax.Identifier.Text, identifier.Text));
    }

    private Func<Func<T1, T2, bool>, bool> CastAndCompare<T1, T2>(T1 sourceType, T2 targetType) {
      return comparison => sourceType != null && targetType != null && comparison(sourceType, targetType);
    }

    public bool Equals(TypeSyntax x, TypeSyntax y) {
      return x != null && y != null && (
        AreEqualPredefinedTypes(x, y)
        || AreEqualIdentifierTypes(x, y)
        || AreEqualGenericTypes(x, y)
        || AreEqualArrayTypes(x, y)
        || AreEqualNullableTypes(x, y)
        || Equals((x as QualifiedNameSyntax)?.Right, y)
        || Equals(x, (y as QualifiedNameSyntax)?.Right)
        );
    }

    public int GetHashCode(TypeSyntax obj) {
      throw new NotImplementedException();
    }

    private bool AreEqualPredefinedTypes(TypeSyntax sourceType, TypeSyntax targetType) {
      return CastAndCompare(sourceType as PredefinedTypeSyntax, targetType as PredefinedTypeSyntax)
        ((s, t) => string.Equals(s.Keyword.Text, t.Keyword.Text));
    }

    private bool AreEqualIdentifierTypes(TypeSyntax sourceType, TypeSyntax targetType) {
      // two types are equal identifiers when the identifier is not provided in the source or target files
      // This is how we differentiate between DateTime and - for example - Department
      return CastAndCompare(sourceType as IdentifierNameSyntax, targetType as IdentifierNameSyntax)
        ((s, t) =>
          string.Equals(s.Identifier.Text, t.Identifier.Text) && !IsSource(s.Identifier) && !IsTarget(t.Identifier));
    }

    private bool AreEqualGenericTypes(TypeSyntax sourceType, TypeSyntax targetType) {
      return CastAndCompare(sourceType as GenericNameSyntax, targetType as GenericNameSyntax)
        ((s, t) => string.Equals(s.Identifier.Text, t.Identifier.Text)
                   && s.TypeArgumentList.Arguments.SequenceEqual(t.TypeArgumentList.Arguments, this));
    }

    private bool AreEqualArrayTypes(TypeSyntax sourceType, TypeSyntax targetType) {
      return CastAndCompare(sourceType as ArrayTypeSyntax, targetType as ArrayTypeSyntax)
        ((s, t) => Equals(s.ElementType, t.ElementType));
    }

    private bool AreEqualNullableTypes(TypeSyntax sourceType, TypeSyntax targetType) {
      return CastAndCompare(sourceType as NullableTypeSyntax, targetType as NullableTypeSyntax)
        ((s, t) => Equals(s.ElementType, t.ElementType));
    }
  }
}