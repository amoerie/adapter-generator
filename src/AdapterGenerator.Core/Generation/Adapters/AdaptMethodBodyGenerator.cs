using System;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdaptMethodBodyGenerator : IAdaptMethodBodyGenerator {
    private readonly IPropertySpecificStatementsGenerator _propertySpecificStatementsGenerator;

    public AdaptMethodBodyGenerator(IPropertySpecificStatementsGenerator propertySpecificStatementsGenerator) {
      if (propertySpecificStatementsGenerator == null)
        throw new ArgumentNullException(nameof(propertySpecificStatementsGenerator));
      _propertySpecificStatementsGenerator = propertySpecificStatementsGenerator;
    }

    public BlockSyntax Generate(IClassAdapterGenerationContextWithClass context) {
      // if (source == null) return null;
      var ifSourceIsNullThenReturnNullStatement = IfStatement(
        BinaryExpression(SyntaxKind.EqualsExpression, IdentifierName("source"),
          LiteralExpression(SyntaxKind.NullLiteralExpression)),
        ReturnStatement(LiteralExpression(SyntaxKind.NullLiteralExpression)));

      // var target = new Target();
      var invokeTargetConstructorAndAssignToVariableStatement =
        LocalDeclarationStatement(VariableDeclaration(IdentifierName("var"))
          .WithVariables(SingletonSeparatedList(
            VariableDeclarator(Identifier("target"))
              .WithInitializer(EqualsValueClause(
                ObjectCreationExpression(context.Blueprint.ClassMatch.Target.QualifiedName)
                  .WithArgumentList(ArgumentList()))))));

      // target.Name = source.Name, etc.
      StatementSyntax[] propertySpecificAdaptationStatements = context.Blueprint.ClassMatch.Target.Properties
        .SelectMany(
          property => _propertySpecificStatementsGenerator.Generate(context, property))
        .ToArray();

      // return target;
      var returnAdaptedTargetVariableStatement = ReturnStatement(IdentifierName("target"));
      return Block(ifSourceIsNullThenReturnNullStatement)
        .AddStatements(invokeTargetConstructorAndAssignToVariableStatement)
        .AddStatements(propertySpecificAdaptationStatements)
        .AddStatements(returnAdaptedTargetVariableStatement);
    }

    public BlockSyntax Generate(IEnumAdapterGenerationContextWithClass context) {
      var matchedValuesSwitchSections = context.Blueprint.EnumMatch.ValueMatches
        .Select(enumValueMatch => SwitchSection()
          .WithLabels(
            SingletonList<SwitchLabelSyntax>(
              CaseSwitchLabel(
                MemberAccessExpression(
                  SyntaxKind.SimpleMemberAccessExpression,
                  enumValueMatch.Source.Enum.QualifiedName,
                  IdentifierName(Identifier(enumValueMatch.Source.EnumMemberDeclarationSyntax.Identifier.Text))))))
          .WithStatements(
            SingletonList<StatementSyntax>(
              ReturnStatement(
                MemberAccessExpression(
                  SyntaxKind.SimpleMemberAccessExpression,
                  enumValueMatch.Target.Enum.QualifiedName,
                  IdentifierName(Identifier(enumValueMatch.Target.EnumMemberDeclarationSyntax.Identifier.Text)))))));
      var cannotAdaptMessage = InterpolatedStringExpression(
          Token(SyntaxKind.InterpolatedStringStartToken))
        .WithContents(
          List(
            new InterpolatedStringContentSyntax[] {
              InterpolatedStringText()
                .WithTextToken(
                  Token(TriviaList(), SyntaxKind.InterpolatedStringTextToken, "Cannot adapt ", "Cannot adapt ",
                    TriviaList())),
              Interpolation(
                IdentifierName("source")),
              InterpolatedStringText()
                .WithTextToken(
                  Token(
                    TriviaList(),
                    SyntaxKind.InterpolatedStringTextToken,
                    " to a matching value",
                    " to a matching value",
                    TriviaList()))
            }));
      var defaultThrowArgumentOutOfRangeStatement =
        ThrowStatement(
          ObjectCreationExpression(
              IdentifierName("ArgumentOutOfRangeException"))
            .WithArgumentList(
              ArgumentList(
                SeparatedList<ArgumentSyntax>(
                  new SyntaxNodeOrToken[] {
                    Argument(InvocationExpression(IdentifierName("nameof")).WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName("source")))))),
                    Token(SyntaxKind.CommaToken),
                    Argument(IdentifierName("source")),
                    Token(SyntaxKind.CommaToken),
                    Argument(cannotAdaptMessage)}))));
      var defaultSwitchSection = SwitchSection()
        .WithLabels(SingletonList<SwitchLabelSyntax>(DefaultSwitchLabel()))
        .WithStatements(SingletonList<StatementSyntax>(defaultThrowArgumentOutOfRangeStatement));
      StatementSyntax switchStatement = SwitchStatement(IdentifierName("source"))
        .WithSections(List(matchedValuesSwitchSections).Add(defaultSwitchSection));
      return Block(switchStatement);
    }
  }
}