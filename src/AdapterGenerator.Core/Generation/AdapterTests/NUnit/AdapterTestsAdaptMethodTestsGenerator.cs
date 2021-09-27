using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsAdaptMethodTestsGenerator : IAdapterTestsAdaptMethodTestsGenerator {
    private readonly IAdapterTestsPropertyTestMethodGenerator _propertyTestMethodGenerator;

    public AdapterTestsAdaptMethodTestsGenerator(IAdapterTestsPropertyTestMethodGenerator propertyTestMethodGenerator) {
      if (propertyTestMethodGenerator == null)
        throw new ArgumentNullException(nameof(propertyTestMethodGenerator));
      _propertyTestMethodGenerator = propertyTestMethodGenerator;
    }

    public ClassDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context) {
      var sourceField = FieldDeclaration(VariableDeclaration(context.Adapter.Blueprint.ClassMatch.Source.QualifiedName)
          .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier("_source")))))
        .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword)));
      var setupMethod = MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("SetUp"))
        .WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("SetUp"))))))
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
        .WithBody(Block(
          ExpressionStatement(
            InvocationExpression(
              MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, BaseExpression(), IdentifierName("SetUp")))),
          ExpressionStatement(
            AssignmentExpression(
              SyntaxKind.SimpleAssignmentExpression,
              IdentifierName("_source"),
              ObjectCreationExpression(context.Adapter.Blueprint.ClassMatch.Source.QualifiedName)
                .WithArgumentList(ArgumentList())))));
      var nullTest = MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)),
          Identifier("ShouldReturnNullWhenSourceIsNull"))
        .WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("Test"))))))
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        .WithBody(
          Block(
            // _source = null;
            ExpressionStatement(AssignmentExpression(
              SyntaxKind.SimpleAssignmentExpression, IdentifierName("_source"),
              LiteralExpression(SyntaxKind.NullLiteralExpression))),
            // var target = _sut.Adapt(_source);
            LocalDeclarationStatement(
              VariableDeclaration(IdentifierName("var"))
                .WithVariables(SingletonSeparatedList(
                  VariableDeclarator(Identifier("target"))
                    .WithInitializer(
                      EqualsValueClause(
                        InvocationExpression(
                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,IdentifierName("_sut"),IdentifierName("Adapt")))
                          .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName("_source")))))))))),
            // target.Should().BeNull();
            ExpressionStatement(
              InvocationExpression(
                MemberAccessExpression(
                  SyntaxKind.SimpleMemberAccessExpression,
                  InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,IdentifierName("target"),IdentifierName("Should"))),
                  IdentifierName("BeNull"))))));
      var propertySpecificTests =
        context.Adapter.Blueprint.ClassMatch.Target.Properties.Select(
          p => _propertyTestMethodGenerator.Generate(context, p));
      return ClassDeclaration("Adapt")
        .WithAttributeLists(
          SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("TestFixture"))))))
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        .WithBaseList(
          BaseList(SingletonSeparatedList<BaseTypeSyntax>(SimpleBaseType(IdentifierName(context.TestClassName)))))
        .WithMembers(List<MemberDeclarationSyntax>()
          .Add(sourceField)
          .Add(setupMethod)
          .Add(nullTest)
          .AddRange(propertySpecificTests));
    }

    public ClassDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithClass context) {
      var enumMatch = context.Adapter.Blueprint.EnumMatch;
      var testCasesAttributes = enumMatch.ValueMatches.Select(enumValueMatch =>
        AttributeList(
          SingletonSeparatedList<AttributeSyntax>(Attribute(
              IdentifierName("TestCase"))
            .WithArgumentList(
              AttributeArgumentList(
                SeparatedList<AttributeArgumentSyntax>(
                  new SyntaxNodeOrToken[] {
                    AttributeArgument(
                      MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        enumMatch.Source.QualifiedName,
                        IdentifierName(Identifier(enumValueMatch.Source.EnumMemberDeclarationSyntax.Identifier.Text)))),
                    Token(SyntaxKind.CommaToken),
                    AttributeArgument(
                      MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        enumMatch.Target.QualifiedName,
                        IdentifierName(Identifier(enumValueMatch.Target.EnumMemberDeclarationSyntax.Identifier.Text))))
                  }))))));
      var testCasesMethod = MethodDeclaration(
          PredefinedType(
            Token(SyntaxKind.VoidKeyword)),
          Identifier("ShouldAdaptCorrectly"))
        .WithAttributeLists(List(testCasesAttributes))
        .WithModifiers(
          TokenList(
            Token(SyntaxKind.PublicKeyword)))
        .WithParameterList(
          ParameterList(
            SeparatedList<ParameterSyntax>(
              new SyntaxNodeOrToken[] {
                Parameter(Identifier("source"))
                  .WithType(enumMatch.Source.QualifiedName),
                Token(SyntaxKind.CommaToken),
                Parameter(Identifier("target"))
                  .WithType(enumMatch.Target.QualifiedName)
              })))
        .WithBody(
          Block(
            SingletonList<StatementSyntax>(
              ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                      SyntaxKind.SimpleMemberAccessExpression,
                      InvocationExpression(
                        MemberAccessExpression(
                          SyntaxKind.SimpleMemberAccessExpression,
                          InvocationExpression(
                              MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("_sut"),
                                IdentifierName("Adapt")))
                            .WithArgumentList(
                              ArgumentList(SingletonSeparatedList(
                                Argument(IdentifierName("source"))))),
                          IdentifierName("Should"))),
                      IdentifierName("Be")))
                  .WithArgumentList(
                    ArgumentList(
                      SingletonSeparatedList(Argument(IdentifierName("target")))))))));
      return ClassDeclaration("Adapt")
        .WithAttributeLists(
          SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("TestFixture"))))))
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        .WithBaseList(
          BaseList(SingletonSeparatedList<BaseTypeSyntax>(SimpleBaseType(IdentifierName(context.TestClassName)))))
        .WithMembers(List<MemberDeclarationSyntax>().Add(testCasesMethod));
    }
  }
}