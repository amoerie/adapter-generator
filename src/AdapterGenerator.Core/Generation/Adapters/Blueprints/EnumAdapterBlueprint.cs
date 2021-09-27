using System;
using AdapterGenerator.Core.Matching.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Blueprints {
  public class EnumAdapterBlueprint : IEnumAdapterBlueprint {
    public IEnumMatch EnumMatch { get; }

    public EnumAdapterBlueprint(IEnumMatch enumMatch) {
      if (enumMatch == null) throw new ArgumentNullException(nameof(enumMatch));
      EnumMatch = enumMatch;
    }

    public EnumDeclarationSyntax SourceType => EnumMatch.Source.EnumDeclarationSyntax;
    public EnumDeclarationSyntax TargetType => EnumMatch.Target.EnumDeclarationSyntax;
    BaseTypeDeclarationSyntax IAdapterBlueprint.SourceType => SourceType;
    BaseTypeDeclarationSyntax IAdapterBlueprint.TargetType => TargetType;

    public IdentifierNameSyntax Name
      => SyntaxFactory.IdentifierName($"{EnumMatch.Target.EnumDeclarationSyntax.Identifier.Text}Adapter");

    public NameSyntax QualifiedName => EnumMatch.Target.NamespaceDeclarationSyntax != null
      ? SyntaxFactory.QualifiedName(EnumMatch.Target.NamespaceDeclarationSyntax.Name, Name) as NameSyntax
      : Name;

    public GenericNameSyntax InterfaceType => SyntaxFactory.GenericName(
      SyntaxFactory.Identifier("IAdapter"))
      .WithTypeArgumentList(
        SyntaxFactory.TypeArgumentList(
          SyntaxFactory.SeparatedList<TypeSyntax>(
            new SyntaxNodeOrToken[] {
              EnumMatch.Source.QualifiedName,
              SyntaxFactory.Token(SyntaxKind.CommaToken),
              EnumMatch.Target.QualifiedName
            })));

    public string ParameterName => $"{char.ToLower(Name.Identifier.Text[0])}{Name.Identifier.Text.Substring(1)}";
    public string FieldName => $"_{ParameterName}";
  }
}