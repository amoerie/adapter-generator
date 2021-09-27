using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class RealProperty : IProperty {
    public RealProperty(IClass @class, PropertyDeclarationSyntax propertyDeclarationSyntax) {
      if (@class == null) throw new ArgumentNullException(nameof(@class));
      Class = @class;
      PropertyDeclarationSyntax = propertyDeclarationSyntax;
    }

    public IClass Class { get; }
    public PropertyDeclarationSyntax PropertyDeclarationSyntax { get; }

    public override string ToString() {
      return $"{Class}.{PropertyDeclarationSyntax.Identifier.Text}";
    }
  }
}