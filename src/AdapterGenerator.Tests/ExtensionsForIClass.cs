using System.Linq;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Tests {
  internal static class ExtensionsForIClass {
    public static IProperty FindPropertyByName(this IClass @class, string propertyName) {
      return @class.Properties.Single(p => p.PropertyDeclarationSyntax.Identifier.Text.Equals(propertyName));
    }
  }
}