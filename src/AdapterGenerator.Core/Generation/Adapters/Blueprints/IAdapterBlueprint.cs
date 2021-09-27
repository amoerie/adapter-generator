using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Blueprints {
  public interface IAdapterBlueprint {
    IdentifierNameSyntax Name { get; }
    NameSyntax QualifiedName { get; }
    GenericNameSyntax InterfaceType { get; }
    BaseTypeDeclarationSyntax SourceType { get; }
    BaseTypeDeclarationSyntax TargetType { get; }
    string ParameterName { get; }
    string FieldName { get; }
  }

  public interface IClassAdapterBlueprint : IAdapterBlueprint {
    IClassMatch ClassMatch { get; }
    IImmutableList<IAdapterBlueprint> NestedAdapters { get; }
    new ClassDeclarationSyntax SourceType { get; }
    new ClassDeclarationSyntax TargetType { get; }
  }

  public interface IEnumAdapterBlueprint : IAdapterBlueprint {
    IEnumMatch EnumMatch { get; }
    new EnumDeclarationSyntax SourceType { get; }
    new EnumDeclarationSyntax TargetType { get; }
  }
}