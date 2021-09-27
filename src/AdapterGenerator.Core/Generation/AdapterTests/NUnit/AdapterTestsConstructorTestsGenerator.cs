using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsConstructorTestsGenerator : IAdapterTestsConstructorTestsGenerator {
    private ClassDeclarationSyntax Generate(string testClassName) {
      var testMethod = SyntaxFactory.MethodDeclaration(
        SyntaxFactory.PredefinedType(
          SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
        SyntaxFactory.Identifier("ShouldHaveNoOptionalDependencies"))
        .WithAttributeLists(
          SyntaxFactory.SingletonList(
            SyntaxFactory.AttributeList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Test"))))))
        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
        .WithBody(
          SyntaxFactory.Block(
            SyntaxFactory.SingletonList<StatementSyntax>(
              SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                  SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.InvocationExpression(
                      SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("_sut"),
                        SyntaxFactory.IdentifierName("Should"))),
                    SyntaxFactory.IdentifierName("HaveExactlyOneConstructorWithoutOptionalParameters")))))));
      return SyntaxFactory.ClassDeclaration("Constructor")
        .WithAttributeLists(
          SyntaxFactory.SingletonList(
            SyntaxFactory.AttributeList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("TestFixture"))))))
        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
        .WithBaseList(
          SyntaxFactory.BaseList(
            SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
              SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(testClassName)))))
        .WithMembers(SyntaxFactory.List(new MemberDeclarationSyntax[] {testMethod}));
    }

    public ClassDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context) {
      return Generate(context.TestClassName);
    }

    public ClassDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithClass context) {
      return Generate(context.TestClassName);
    }
  }
}