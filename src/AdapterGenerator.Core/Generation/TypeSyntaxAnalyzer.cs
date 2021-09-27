using System;
using System.Linq;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation {
  public class TypeSyntaxAnalyzer : ITypeSyntaxAnalyzer {
    private TOut CastAndConvert<TIn, TOut>(TIn type, Func<TIn, TOut> convert) {
      return type != null ? convert(type) : default(TOut);
    }

    public bool IsArray(TypeSyntax typeSyntax) {
      return typeSyntax is ArrayTypeSyntax;
    }

    public bool IsIEnumerable(TypeSyntax typeSyntax) {
      return CastAndConvert(typeSyntax as GenericNameSyntax,
        t => string.Equals(t.Identifier.Text, PropertyTypes.IEnumerable));
    }

    public bool IsCollectionType(TypeSyntax typeSyntax) {
      return CastAndConvert(typeSyntax as GenericNameSyntax,
        t => PropertyTypes.KnownCollectionTypes.Contains(t.Identifier.Text));
    }

    public bool ImplementsIEnumerable(TypeSyntax typeSyntax) {
      return CastAndConvert(typeSyntax as GenericNameSyntax,
        t => PropertyTypes.KnownEnumerableTypes.Contains(t.Identifier.Text))
             || CastAndConvert(typeSyntax as ArrayTypeSyntax, t => true);
    }

    public TypeSyntax ExtractElementTypeFromEnumerable(TypeSyntax type) {
      return CastAndConvert(type as ArrayTypeSyntax, t => t.ElementType)
             ??
             CastAndConvert(type as GenericNameSyntax,
               t => PropertyTypes.KnownEnumerableTypes.Contains(t.Identifier.Text)
                 ? t.TypeArgumentList.Arguments.Single()
                 : null);
    }
  }
}