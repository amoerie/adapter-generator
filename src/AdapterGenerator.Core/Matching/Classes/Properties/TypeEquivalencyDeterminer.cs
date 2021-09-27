using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Matching.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class TypeEquivalencyDeterminer : ITypeEquivalencyDeterminer, IEqualityComparer<TypeSyntax> {
    private readonly IImmutableList<IClassMatchWithoutPropertyMatches> _classMatches;
    private readonly IImmutableList<IEnumMatch> _enumMatches;

    public TypeEquivalencyDeterminer(IImmutableList<IClassMatchWithoutPropertyMatches> classMatches,
      IImmutableList<IEnumMatch> enumMatches) {
      if (classMatches == null) throw new ArgumentNullException(nameof(classMatches));
      if (enumMatches == null) throw new ArgumentNullException(nameof(enumMatches));
      _classMatches = classMatches;
      _enumMatches = enumMatches;
    }

    public bool AreEquivalent(TypeSyntax sourceType, TypeSyntax targetType) {
      // flatten down nullable types to their inner type, DateTime? will match DateTime, etc.
      sourceType = RemoveNullableDecoration(sourceType);
      targetType = RemoveNullableDecoration(targetType);
      return sourceType != null && targetType != null &&
             // string matches with string, ...
             (AreEquivalentPredefinedTypes(sourceType as PredefinedTypeSyntax, targetType as PredefinedTypeSyntax)
               // Address matches with address.
              || AreEquivalentIdentifierTypes(sourceType as IdentifierNameSyntax, targetType as IdentifierNameSyntax)
               // Blabla<string> matches with Blabla<string>, ...
              || AreEquivalentGenericTypes(sourceType as GenericNameSyntax, targetType as GenericNameSyntax)
               // Address[] matches with Address[], ...
              || AreEquivalentArrayTypes(sourceType as ArrayTypeSyntax, targetType as ArrayTypeSyntax)
               // IEnumerable<string> matches with string[], ...
              || AreEquivalentEnumerableTypes(sourceType, targetType)
               // System.Guid matches with Guid
              || AreEquivalent((sourceType as QualifiedNameSyntax)?.Right, targetType)
              || AreEquivalent(sourceType, (targetType as QualifiedNameSyntax)?.Right));
    }

    private bool IsKnownClassMatch(string sourceIdentifier, string targetIdentifier) {
      return _classMatches.Any(cm => string.Equals(cm.Source.ClassDeclarationSyntax.Identifier.Text, sourceIdentifier)
                                     &&
                                     string.Equals(cm.Target.ClassDeclarationSyntax.Identifier.Text, targetIdentifier));
    }

    private bool IsKnownEnumMatch(string sourceIdentifier, string targetIdentifier) {
      return _enumMatches.Any(cm => string.Equals(cm.Source.EnumDeclarationSyntax.Identifier.Text, sourceIdentifier)
                                    &&
                                    string.Equals(cm.Target.EnumDeclarationSyntax.Identifier.Text, targetIdentifier));
    }

    private TypeSyntax RemoveNullableDecoration(TypeSyntax type) {
      return (type as NullableTypeSyntax)?.ElementType ?? type;
    }

    private bool AreEquivalentPredefinedTypes(PredefinedTypeSyntax sourceType, PredefinedTypeSyntax targetType) {
      return sourceType != null
             && targetType != null
             && string.Equals(sourceType.Keyword.Text, targetType.Keyword.Text);
    }

    private bool AreEquivalentIdentifierTypes(IdentifierNameSyntax sourceType, IdentifierNameSyntax targetType) {
      var sourceIdentifier = sourceType?.Identifier.Text;
      var targetIdentifier = targetType?.Identifier.Text;
      return sourceIdentifier != null
             && targetIdentifier != null
             &&
             (
               string.Equals(sourceIdentifier, targetIdentifier)
               || IsKnownClassMatch(sourceIdentifier, targetIdentifier)
               || IsKnownEnumMatch(sourceIdentifier, targetIdentifier)
               );
    }

    private bool AreEquivalentGenericTypes(GenericNameSyntax sourceType, GenericNameSyntax targetType) {
      return sourceType != null
             && targetType != null
             && string.Equals(sourceType.Identifier.Text, targetType.Identifier.Text)
             && sourceType.TypeArgumentList.Arguments.SequenceEqual(targetType.TypeArgumentList.Arguments, this);
    }

    private bool AreEquivalentArrayTypes(ArrayTypeSyntax sourceType, ArrayTypeSyntax targetType) {
      return sourceType != null
             && targetType != null
             && AreEquivalent(sourceType.ElementType, targetType.ElementType);
    }

    private bool AreEquivalentEnumerableTypes(TypeSyntax sourceType, TypeSyntax targetType) {
      var sourceElementType = ExtractEnumerableElementType(sourceType);
      var targetElementType = ExtractEnumerableElementType(targetType);
      return sourceElementType != null
             && targetElementType != null
             && AreEquivalent(sourceElementType, targetElementType);
    }

    private TypeSyntax ExtractEnumerableElementType(TypeSyntax type) {
      var arrayType = type as ArrayTypeSyntax;
      if (arrayType != null) return arrayType.ElementType;
      var genericNameSyntax = type as GenericNameSyntax;
      if (genericNameSyntax == null) return null;
      return PropertyTypes.KnownEnumerableTypes.Contains(genericNameSyntax.Identifier.Text)
        ? genericNameSyntax.TypeArgumentList.Arguments.Single()
        : null;
    }

    public bool Equals(TypeSyntax x, TypeSyntax y) {
      return AreEquivalent(x, y);
    }

    public int GetHashCode(TypeSyntax typeSyntax) {
      return typeSyntax.GetHashCode();
    }
  }
}