using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public interface IClass {
    CompilationUnitSyntax CompilationUnitSyntax { get; }
    NamespaceDeclarationSyntax NamespaceDeclarationSyntax { get; }
    ClassDeclarationSyntax ClassDeclarationSyntax { get; }
    ImmutableList<IProperty> Properties { get; }
    NameSyntax QualifiedName { get; }
  }
}