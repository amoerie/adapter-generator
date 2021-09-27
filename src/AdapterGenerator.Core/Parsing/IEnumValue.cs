using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public interface IEnumValue {
    IEnum Enum { get; }
    EnumMemberDeclarationSyntax EnumMemberDeclarationSyntax { get; }
  }
}