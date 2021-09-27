using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class RealEnum : IEnum {
    public CompilationUnitSyntax CompilationUnitSyntax { get; }
    public NamespaceDeclarationSyntax NamespaceDeclarationSyntax { get; }
    public EnumDeclarationSyntax EnumDeclarationSyntax { get; }

    public RealEnum(CompilationUnitSyntax compilationUnitSyntax, NamespaceDeclarationSyntax namespaceDeclarationSyntax,
      EnumDeclarationSyntax enumDeclarationSyntax) {
      if (compilationUnitSyntax == null) throw new ArgumentNullException(nameof(compilationUnitSyntax));
      if (enumDeclarationSyntax == null) throw new ArgumentNullException(nameof(enumDeclarationSyntax));
      CompilationUnitSyntax = compilationUnitSyntax;
      NamespaceDeclarationSyntax = namespaceDeclarationSyntax;
      EnumDeclarationSyntax = enumDeclarationSyntax;
      QualifiedName = new QualifiedNameFactory().Create(EnumDeclarationSyntax);
      Values = enumDeclarationSyntax.Members.Select(m => new RealEnumValue(this, m)).ToImmutableList<IEnumValue>();
    }

    public NameSyntax QualifiedName { get; }
    public IImmutableList<IEnumValue> Values { get; }

    public override string ToString() {
      return $"{EnumDeclarationSyntax.Identifier.Text}";
    }
  }
}