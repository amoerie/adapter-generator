using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class RealEnumValue : IEnumValue {
    public IEnum Enum { get; }
    public EnumMemberDeclarationSyntax EnumMemberDeclarationSyntax { get; }

    public RealEnumValue(IEnum @enum, EnumMemberDeclarationSyntax enumMemberDeclarationSyntax) {
      if (@enum == null) throw new ArgumentNullException(nameof(@enum));
      if (enumMemberDeclarationSyntax == null) throw new ArgumentNullException(nameof(enumMemberDeclarationSyntax));
      Enum = @enum;
      EnumMemberDeclarationSyntax = enumMemberDeclarationSyntax;
    }

    public override string ToString() {
      return $"{Enum}.{EnumMemberDeclarationSyntax.Identifier.Text}";
    }
  }
}