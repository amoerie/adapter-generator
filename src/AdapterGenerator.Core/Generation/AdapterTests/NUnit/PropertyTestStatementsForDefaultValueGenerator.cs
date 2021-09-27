using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class PropertyTestStatementsForDefaultValueGenerator : IPropertyTestStatementsGenerator {
    private readonly IClassAdapterTestsGenerationContextWithClass _context;
    private readonly IProperty _targetProperty;

    public PropertyTestStatementsForDefaultValueGenerator(IClassAdapterTestsGenerationContextWithClass context,
      IProperty targetProperty) {
      if (context == null) throw new ArgumentNullException(nameof(context));
      if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));
      _context = context;
      _targetProperty = targetProperty;
    }

    public IImmutableList<StatementSyntax> Generate() {
      // var target = _sut.Adapt(_source);
      var varTargetIsAdaptSource = SyntaxFactory.LocalDeclarationStatement(
        SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"))
          .WithVariables(SyntaxFactory.SingletonSeparatedList(
            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("target"))
              .WithInitializer(
                SyntaxFactory.EqualsValueClause(
                  SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                      SyntaxKind.SimpleMemberAccessExpression,
                      SyntaxFactory.IdentifierName("_sut"),
                      SyntaxFactory.IdentifierName("Adapt")))
                    .WithArgumentList(
                      SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                          SyntaxFactory.Argument(SyntaxFactory.IdentifierName("_source"))))))))));
      // target.Property.Should().Be(default(PropertyType)); // TODO
      var propertyShouldBeDefaultValue = SyntaxFactory.ExpressionStatement(
        SyntaxFactory.InvocationExpression(
          SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.InvocationExpression(
              SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.MemberAccessExpression(
                  SyntaxKind.SimpleMemberAccessExpression,
                  SyntaxFactory.IdentifierName("target"),
                  SyntaxFactory.IdentifierName(_targetProperty.PropertyDeclarationSyntax.Identifier.Text)),
                SyntaxFactory.IdentifierName("Should"))),
            SyntaxFactory.IdentifierName("Be")))
          .WithArgumentList(
            SyntaxFactory.ArgumentList(
              SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Argument(SyntaxFactory.DefaultExpression(_targetProperty.PropertyDeclarationSyntax.Type))))))
        .WithSemicolonToken(
          SyntaxFactory.Token(SyntaxFactory.TriviaList(), SyntaxKind.SemicolonToken,
            SyntaxFactory.TriviaList(SyntaxFactory.Comment("// TODO"))));
      return ImmutableList.Create<StatementSyntax>(varTargetIsAdaptSource, propertyShouldBeDefaultValue);
    }
  }
}