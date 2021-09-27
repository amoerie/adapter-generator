using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public class SimpleAssignmentStatementsGenerator : IPropertyAdaptingStatementsGenerator {
    private readonly IClassAdapterGenerationContextWithClass _classAdapterGenerationContext;
    private readonly IPropertyMatch _propertyMatch;

    public SimpleAssignmentStatementsGenerator(IClassAdapterGenerationContextWithClass classAdapterGenerationContext,
      IPropertyMatch propertyMatch) {
      if (classAdapterGenerationContext == null)
        throw new ArgumentNullException(nameof(classAdapterGenerationContext));
      if (propertyMatch == null) throw new ArgumentNullException(nameof(propertyMatch));
      _classAdapterGenerationContext = classAdapterGenerationContext;
      _propertyMatch = propertyMatch;
    }

    public IImmutableList<StatementSyntax> Generate() {
      var simpleAssignmentExpression = SyntaxFactory.ExpressionStatement(
        SyntaxFactory.AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.IdentifierName("target"),
            SyntaxFactory.IdentifierName(_propertyMatch.Target.PropertyDeclarationSyntax.Identifier)),
          SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.IdentifierName("source"),
            SyntaxFactory.IdentifierName(_propertyMatch.Source.PropertyDeclarationSyntax.Identifier))));
      return ImmutableList.Create<StatementSyntax>(simpleAssignmentExpression);
    }
  }
}