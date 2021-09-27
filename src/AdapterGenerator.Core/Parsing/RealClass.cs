using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Parsing {
  public class RealClass : IClass {
    public RealClass(CompilationUnitSyntax compilationUnitSyntax, NamespaceDeclarationSyntax namespaceDeclarationSyntax,
      ClassDeclarationSyntax classDeclarationSyntax) {
      if (compilationUnitSyntax == null) throw new ArgumentNullException(nameof(compilationUnitSyntax));
      if (classDeclarationSyntax == null) throw new ArgumentNullException(nameof(classDeclarationSyntax));
      CompilationUnitSyntax = compilationUnitSyntax;
      NamespaceDeclarationSyntax = namespaceDeclarationSyntax;
      ClassDeclarationSyntax = classDeclarationSyntax;
      QualifiedName = new QualifiedNameFactory().Create(ClassDeclarationSyntax);
      Properties = classDeclarationSyntax.Members.OfType<PropertyDeclarationSyntax>()
        .Select(p => new RealProperty(this, p) as IProperty)
        .ToImmutableList();
    }

    public CompilationUnitSyntax CompilationUnitSyntax { get; }
    public NamespaceDeclarationSyntax NamespaceDeclarationSyntax { get; }
    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }
    public ImmutableList<IProperty> Properties { get; }
    public NameSyntax QualifiedName { get; }

    public override string ToString() {
      return $"{ClassDeclarationSyntax.Identifier.Text}";
    }
  }
}