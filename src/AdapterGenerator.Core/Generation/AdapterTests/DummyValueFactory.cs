using System;
using System.Linq;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public class DummyValueFactory : IDummyValueFactory {
    private ExpressionSyntax CreateDummyString() {
      return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("abc"));
    }

    private ExpressionSyntax CreateDummyBoolean() {
      return LiteralExpression(SyntaxKind.TrueLiteralExpression);
    }

    private ExpressionSyntax CreateDummyChar() {
      return LiteralExpression(SyntaxKind.CharacterLiteralExpression, Literal('x'));
    }

    private ExpressionSyntax CreateDummyNumber() {
      return LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(42));
    }

    private ExpressionSyntax CreateDummyDateTime() {
      return ObjectCreationExpression(IdentifierName("DateTime"))
        .WithArgumentList(
          ArgumentList(
            SeparatedList<ArgumentSyntax>(
              new SyntaxNodeOrToken[] {
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(1990))),
                Token(SyntaxKind.CommaToken),
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(6))),
                Token(SyntaxKind.CommaToken),
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(2)))
              })));
    }

    private ExpressionSyntax CreateDummyTimeSpan() {
      return ObjectCreationExpression(
          IdentifierName("TimeSpan"))
        .WithArgumentList(
          ArgumentList(
            SeparatedList<ArgumentSyntax>(
              new SyntaxNodeOrToken[] {
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(1))),
                Token(SyntaxKind.CommaToken),
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(2))),
                Token(SyntaxKind.CommaToken),
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(3))),
                Token(SyntaxKind.CommaToken),
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(4))),
                Token(SyntaxKind.CommaToken),
                Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(5)))
              })));
    }

    private ExpressionSyntax CreateDummyGuid() {
      return InvocationExpression(
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName("Guid"),
            IdentifierName("Parse")))
        .WithArgumentList(
          ArgumentList(
            SingletonSeparatedList<ArgumentSyntax>(
              Argument(
                LiteralExpression(
                  SyntaxKind.StringLiteralExpression,
                  Literal("f38e87b4-ce6f-4ba6-8516-548df19efd1b"))))));
    }

    private TypeSyntax ToQualifiedNameIfPossible(ITypeDeclarations typeDeclarations, TypeSyntax type) {
      var identifierName = type as IdentifierNameSyntax;
      if (identifierName == null) return type;

      // if it's one of the enums, return an enum value
      var @enum = typeDeclarations.Enums.FirstOrDefault(
        e => string.Equals(e.EnumDeclarationSyntax.Identifier.Text, identifierName.Identifier.Text));
      if (@enum != null)
        return @enum.QualifiedName;

      var @class =
        typeDeclarations.Classes.FirstOrDefault(
          c => string.Equals(c.ClassDeclarationSyntax.Identifier.Text, identifierName.Identifier.Text));
      if (@class != null)
        return @class.QualifiedName;
      return type;
    }

    private ExpressionSyntax CreateDummyArray(ITypeDeclarations typeDeclarations, TypeSyntax elementType) {
      return ArrayCreationExpression(
          ArrayType(ToQualifiedNameIfPossible(typeDeclarations, elementType))
            .WithRankSpecifiers(
              SingletonList(
                ArrayRankSpecifier(
                  SingletonSeparatedList<ExpressionSyntax>(
                    OmittedArraySizeExpression())))))
        .WithInitializer(
          InitializerExpression(
            SyntaxKind.ArrayInitializerExpression,
            SeparatedList(
              new [] {
                CreateDummyValue(typeDeclarations, elementType),
                CreateDummyValue(typeDeclarations, elementType),
                CreateDummyValue(typeDeclarations, elementType),
              }
            )
          )
        );
    }

    private ExpressionSyntax CreateDummyIEnumerableOf(ITypeDeclarations typeDeclarations, TypeSyntax elementType) {
      return CreateDummyArray(typeDeclarations, elementType);
    }

    private ExpressionSyntax CreateDummyIListOf(ITypeDeclarations typeDeclarations, TypeSyntax elementType) {
      return InvocationExpression(
        MemberAccessExpression(
          SyntaxKind.SimpleMemberAccessExpression,
          CreateDummyArray(typeDeclarations, elementType),
          IdentifierName("ToList")));
    }

    private ExpressionSyntax CreateDummyValue(IEnum @enum) {
      var firstMember = @enum.EnumDeclarationSyntax.Members.FirstOrDefault();
      return firstMember != null
        ? (ExpressionSyntax) MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, @enum.QualifiedName,
          IdentifierName(firstMember.Identifier))
        : DefaultExpression(@enum.QualifiedName);
    }

    private ExpressionSyntax CreateDummyValue(IClass @class) {
      return ObjectCreationExpression(@class.QualifiedName).WithArgumentList(ArgumentList());
    }

    private ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, IdentifierNameSyntax identifierName) {
      // if any of the known identifiers, we know what to do
      switch (identifierName.Identifier.Text) {
        case "DateTime":
          return CreateDummyDateTime();
        case "TimeSpan":
          return CreateDummyTimeSpan();
        case "Guid":
          return CreateDummyGuid();
      }
      // if it's one of the enums, return an enum value
      var @enum =
        typeDeclarations.Enums.FirstOrDefault(
          e => string.Equals(e.EnumDeclarationSyntax.Identifier.Text, identifierName.Identifier.Text));
      if (@enum != null)
        return CreateDummyValue(@enum);

      var @class =
        typeDeclarations.Classes.FirstOrDefault(
          c => string.Equals(c.ClassDeclarationSyntax.Identifier.Text, identifierName.Identifier.Text));
      if (@class != null)
        return CreateDummyValue(@class);
      // if all else fails, just return new Blabla();
      return ObjectCreationExpression(identifierName).WithArgumentList(ArgumentList());
    }

    private ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, PredefinedTypeSyntax predefinedType) {
      switch (predefinedType.Keyword.Text.ToLower()) {
        case "string":
          return CreateDummyString();
        case "bool":
          return CreateDummyBoolean();
        case "decimal":
        case "double":
        case "int":
        case "long":
        case "short":
          return CreateDummyNumber();
        case "char":
          return CreateDummyChar();
        default:
          // Not sure what to do here
          return DefaultExpression(predefinedType);
      }
    }

    private ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, NullableTypeSyntax nullableType) {
      return CreateDummyValue(typeDeclarations, nullableType.ElementType);
    }

    private ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, GenericNameSyntax genericName) {
      switch (genericName.Identifier.Text) {
        case "IEnumerable":
          return CreateDummyIEnumerableOf(typeDeclarations, genericName.TypeArgumentList.Arguments.Single());
        case "IList":
        case "List":
        case "ICollection":
        case "Collection":
          return CreateDummyIListOf(typeDeclarations, genericName.TypeArgumentList.Arguments.Single());
        default:
          return DefaultExpression(genericName);
      }
    }

    private ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, ArrayTypeSyntax arrayType) {
      return CreateDummyArray(typeDeclarations, arrayType.ElementType);
    }

    public ExpressionSyntax CreateDummyValue(ITypeDeclarations typeDeclarations, TypeSyntax type) {
      var predefinedElementType = type as PredefinedTypeSyntax;
      if (predefinedElementType != null)
        return CreateDummyValue(typeDeclarations, predefinedElementType);
      var identifierName = type as IdentifierNameSyntax;
      if (identifierName != null)
        return CreateDummyValue(typeDeclarations, identifierName);
      var genericType = type as GenericNameSyntax;
      if (genericType != null)
        return CreateDummyValue(typeDeclarations, genericType);
      var nullableType = type as NullableTypeSyntax;
      if (nullableType != null)
        return CreateDummyValue(typeDeclarations, nullableType);
      var arrayType = type as ArrayTypeSyntax;
      if (arrayType != null)
        return CreateDummyValue(typeDeclarations, arrayType);
      return DefaultExpression(type);
    }
  }
}