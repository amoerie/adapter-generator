using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Blueprints {
  public class ClassAdapterBlueprint : IClassAdapterBlueprint {
    public IClassMatch ClassMatch { get; }
    public IImmutableList<IAdapterBlueprint> NestedAdapters { get; }
    public ClassDeclarationSyntax SourceType => ClassMatch.Source.ClassDeclarationSyntax;
    public ClassDeclarationSyntax TargetType => ClassMatch.Target.ClassDeclarationSyntax;
    BaseTypeDeclarationSyntax IAdapterBlueprint.SourceType => SourceType;
    BaseTypeDeclarationSyntax IAdapterBlueprint.TargetType => TargetType;

    public ClassAdapterBlueprint(IClassMatch classMatch, IImmutableList<IAdapterBlueprint> nestedAdapters) {
      if (classMatch == null) throw new ArgumentNullException(nameof(classMatch));
      if (nestedAdapters == null) throw new ArgumentNullException(nameof(nestedAdapters));
      ClassMatch = classMatch;
      NestedAdapters = nestedAdapters;
    }

    public IdentifierNameSyntax Name
      => SyntaxFactory.IdentifierName($"{ClassMatch.Target.ClassDeclarationSyntax.Identifier.Text}Adapter");

    public NameSyntax QualifiedName => ClassMatch.Target.NamespaceDeclarationSyntax != null
      ? SyntaxFactory.QualifiedName(ClassMatch.Target.NamespaceDeclarationSyntax.Name, Name) as NameSyntax
      : Name;

    public GenericNameSyntax InterfaceType => SyntaxFactory.GenericName(
      SyntaxFactory.Identifier("IAdapter"))
      .WithTypeArgumentList(
        SyntaxFactory.TypeArgumentList(
          SyntaxFactory.SeparatedList<TypeSyntax>(
            new SyntaxNodeOrToken[] {
              ClassMatch.Source.QualifiedName,
              SyntaxFactory.Token(SyntaxKind.CommaToken),
              ClassMatch.Target.QualifiedName
            })));

    public string ParameterName => $"{char.ToLower(Name.Identifier.Text[0])}{Name.Identifier.Text.Substring(1)}";
    public string FieldName => $"_{ParameterName}";
  }
}