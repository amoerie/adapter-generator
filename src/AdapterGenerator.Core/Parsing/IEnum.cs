using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public interface IEnum {
    CompilationUnitSyntax CompilationUnitSyntax { get; }
    NamespaceDeclarationSyntax NamespaceDeclarationSyntax { get; }
    EnumDeclarationSyntax EnumDeclarationSyntax { get; }
    NameSyntax QualifiedName { get; }
    IImmutableList<IEnumValue> Values { get; }
  }
}