using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public interface IProperty {
    /// <summary>
    /// Gets the class that owns this property
    /// </summary>
    IClass Class { get; }

    PropertyDeclarationSyntax PropertyDeclarationSyntax { get; }
  }
}