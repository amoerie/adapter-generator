using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsClassFieldsGenerator : IAdapterTestsClassFieldsGenerator {
    public IImmutableList<FieldDeclarationSyntax> Generate(IClassAdapterTestsGenerationContextWithClass context) {
      var systemUnderTestField = SyntaxFactory.FieldDeclaration(
        SyntaxFactory.VariableDeclaration(context.Adapter.Blueprint.QualifiedName)
          .WithVariables(
            SyntaxFactory.SingletonSeparatedList(
              SyntaxFactory.VariableDeclarator(
                SyntaxFactory.Identifier("_sut")))))
        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));
      var nestedAdapterFields = context.Adapter.Blueprint.NestedAdapters
        .Select(nestedAdapter => SyntaxFactory.FieldDeclaration(
          SyntaxFactory.VariableDeclaration(nestedAdapter.InterfaceType)
            .WithVariables(
              SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.VariableDeclarator((string) nestedAdapter.FieldName))))
          .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))));
      return ImmutableList.Create(systemUnderTestField).AddRange(nestedAdapterFields);
    }

    public IImmutableList<FieldDeclarationSyntax> Generate(IEnumAdapterTestsGenerationContextWithClass context) {
      var systemUnderTestField = SyntaxFactory.FieldDeclaration(
              SyntaxFactory.VariableDeclaration(context.Adapter.Blueprint.QualifiedName)
                .WithVariables(
                  SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.VariableDeclarator(
                      SyntaxFactory.Identifier("_sut")))))
              .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));
      return ImmutableList.Create(systemUnderTestField);
    }
  }
}