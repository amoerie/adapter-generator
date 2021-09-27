using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public class NestedAdapterStatementsGenerator : IPropertyAdaptingStatementsGenerator {
    private readonly IClassAdapterGenerationContextWithClass _classAdapterGenerationContext;
    private readonly IPropertyMatch _propertyMatch;
    private readonly IAdapterBlueprint _nestedAdapter;

    public NestedAdapterStatementsGenerator(IClassAdapterGenerationContextWithClass classAdapterGenerationContext,
      IPropertyMatch propertyMatch, IAdapterBlueprint nestedAdapter) {
      if (classAdapterGenerationContext == null)
        throw new ArgumentNullException(nameof(classAdapterGenerationContext));
      if (propertyMatch == null) throw new ArgumentNullException(nameof(propertyMatch));
      if (nestedAdapter == null) throw new ArgumentNullException(nameof(nestedAdapter));
      _classAdapterGenerationContext = classAdapterGenerationContext;
      _propertyMatch = propertyMatch;
      _nestedAdapter = nestedAdapter;
    }

    public IImmutableList<StatementSyntax> Generate() {
      var targetPropertyIdentifier = _propertyMatch.Target.PropertyDeclarationSyntax.Identifier;
      var adaptWithNestedAdapterStatement = SyntaxFactory.ExpressionStatement(
        SyntaxFactory.AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.IdentifierName("target"),
            SyntaxFactory.IdentifierName(targetPropertyIdentifier)),
          SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              SyntaxFactory.IdentifierName(_nestedAdapter.FieldName),
              SyntaxFactory.IdentifierName("Adapt")))
            .WithArgumentList(
              SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                  SyntaxFactory.Argument(
                    SyntaxFactory.MemberAccessExpression(
                      SyntaxKind.SimpleMemberAccessExpression,
                      SyntaxFactory.IdentifierName("source"),
                      SyntaxFactory.IdentifierName(_propertyMatch.Source.PropertyDeclarationSyntax.Identifier))))))));
      return ImmutableList.Create<StatementSyntax>(adaptWithNestedAdapterStatement);
    }
  }
}