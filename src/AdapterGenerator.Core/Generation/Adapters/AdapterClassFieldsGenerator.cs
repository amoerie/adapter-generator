using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdapterClassFieldsGenerator : IAdapterClassFieldsGenerator {
    public IImmutableList<FieldDeclarationSyntax> Generate(IClassAdapterGenerationContextWithClass context) {
      var nestedAdapters = context.Blueprint.NestedAdapters;
      var fieldDeclarations = nestedAdapters
        .Select(nestedAdapter =>
          FieldDeclaration(
            VariableDeclaration(nestedAdapter.InterfaceType)
              .WithVariables(
                SingletonSeparatedList(
                  VariableDeclarator(
                    Identifier(nestedAdapter.FieldName)))))
            .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword))));

      return ImmutableList.CreateRange(fieldDeclarations);
    }
  }
}