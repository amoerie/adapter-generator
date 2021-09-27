using System.Linq;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Tests {
  internal static class ExtensionsForIEnum {
    public static IEnumValue FindEnumValueByName(this IEnum @enum, string name) {
      return @enum.Values.Single(p => p.EnumMemberDeclarationSyntax.Identifier.Text.Equals(name));
    }
  }
}