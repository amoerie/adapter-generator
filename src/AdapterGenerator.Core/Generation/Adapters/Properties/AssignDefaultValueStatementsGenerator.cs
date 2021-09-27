using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public class AssignDefaultValueStatementsGenerator : IPropertyAdaptingStatementsGenerator {
    private readonly IClassAdapterGenerationContextWithClass _classAdapterGenerationContext;
    private readonly IProperty _targetProperty;

    public AssignDefaultValueStatementsGenerator(IClassAdapterGenerationContextWithClass classAdapterGenerationContext,
      IProperty targetProperty) {
      if (classAdapterGenerationContext == null)
        throw new ArgumentNullException(nameof(classAdapterGenerationContext));
      if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));
      _classAdapterGenerationContext = classAdapterGenerationContext;
      _targetProperty = targetProperty;
    }

    public IImmutableList<StatementSyntax> Generate() {
      var assignPropertyWithDefaultValueExpression =
        ExpressionStatement(AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName("target"),
            IdentifierName(_targetProperty.PropertyDeclarationSyntax.Identifier)),
          DefaultExpression(_targetProperty.PropertyDeclarationSyntax.Type)))
          .WithSemicolonToken(Token(TriviaList(), SyntaxKind.SemicolonToken,
            TriviaList(Comment("// TODO adapt this manually"))));
      return ImmutableList.Create<StatementSyntax>(assignPropertyWithDefaultValueExpression);
    }
  }
}