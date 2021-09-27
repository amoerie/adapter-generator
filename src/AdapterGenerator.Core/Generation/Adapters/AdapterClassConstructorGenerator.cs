using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdapterClassConstructorGenerator : IAdapterClassConstructorGenerator {
    public ConstructorDeclarationSyntax Generate(IClassAdapterGenerationContextWithClass context) {
      var blueprint = context.Blueprint;
      var constructorParameters = blueprint.NestedAdapters
        .Select(
          nestedAdapter => Parameter(Identifier(nestedAdapter.ParameterName)).WithType(nestedAdapter.InterfaceType));
      IEnumerable<StatementSyntax> ifNullThenThrowStatements = blueprint.NestedAdapters
        .Select(nestedAdapter =>
          IfStatement(
            BinaryExpression(
              SyntaxKind.EqualsExpression,
              IdentifierName(nestedAdapter.ParameterName),
              LiteralExpression(SyntaxKind.NullLiteralExpression)),
            ThrowStatement(
              ObjectCreationExpression(
                IdentifierName("ArgumentNullException"))
                .WithArgumentList(
                  ArgumentList(
                    SingletonSeparatedList(
                      Argument(InvocationExpression(IdentifierName("nameof"))
                        .WithArgumentList(ArgumentList(SingletonSeparatedList(
                          Argument(IdentifierName(nestedAdapter.ParameterName))))))))))));

      IEnumerable<StatementSyntax> assignConstructorParameterToFieldStatements = blueprint.NestedAdapters
        .Select(nestedAdapter =>
          ExpressionStatement(
            AssignmentExpression(
              SyntaxKind.SimpleAssignmentExpression,
              IdentifierName(nestedAdapter.FieldName),
              IdentifierName(nestedAdapter.ParameterName))));
      return ConstructorDeclaration(blueprint.Name.Identifier)
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        .WithParameterList(ParameterList(SeparatedList(constructorParameters)))
        .WithBody(Block(ifNullThenThrowStatements.Concat(assignConstructorParameterToFieldStatements)));
    }
  }
}