using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public interface IQualifiedNameFactory {
    NameSyntax Create(BaseTypeDeclarationSyntax type);
  }
}