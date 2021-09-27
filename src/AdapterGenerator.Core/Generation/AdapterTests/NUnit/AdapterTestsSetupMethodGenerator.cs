using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsSetupMethodGenerator : IAdapterTestsSetupMethodGenerator {
    public MethodDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context) {
      // _nestedAdapter = _nestedAdapter.Fake();
      IEnumerable<StatementSyntax> assignFakeValuesToNestedAdapters = context.Adapter.Blueprint.NestedAdapters
        .Select(nestedAdapter => SyntaxFactory.ExpressionStatement(
          SyntaxFactory.AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            SyntaxFactory.IdentifierName((string) nestedAdapter.FieldName),
            SyntaxFactory.InvocationExpression(
              SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName((string) nestedAdapter.FieldName),
                SyntaxFactory.IdentifierName("Fake"))))));
      // _sut = new Adapter(_nestedAdapter1, _nestedAdapter2, ...)
      var sutConstructorArguments = context.Adapter.Blueprint.NestedAdapters
        .Select(nestedAdapter => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(nestedAdapter.FieldName)));
      StatementSyntax initializeSut = SyntaxFactory.ExpressionStatement(
        SyntaxFactory.AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          SyntaxFactory.IdentifierName("_sut"),
          SyntaxFactory.ObjectCreationExpression(
            context.Adapter.Blueprint.QualifiedName)
            .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(sutConstructorArguments)))));
      var setupBody = SyntaxFactory.Block(assignFakeValuesToNestedAdapters.ToArray()).AddStatements(initializeSut);
      return SyntaxFactory.MethodDeclaration(
        // return void
        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
        // method name
        SyntaxFactory.Identifier("SetUp"))
        // [SetUp] attribute
        .WithAttributeLists(
          SyntaxFactory.SingletonList(
            SyntaxFactory.AttributeList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("SetUp"))))))
        // public virtual
        .WithModifiers(
          SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
            SyntaxFactory.Token(SyntaxKind.VirtualKeyword)))
        .WithBody(setupBody);
    }

    public MethodDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithClass context) {
      // _sut = new Adapter()
      StatementSyntax initializeSut = SyntaxFactory.ExpressionStatement(
        SyntaxFactory.AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          SyntaxFactory.IdentifierName("_sut"),
          SyntaxFactory.ObjectCreationExpression(
            context.Adapter.Blueprint.QualifiedName)
            .WithArgumentList(SyntaxFactory.ArgumentList())));
      var setupBody = SyntaxFactory.Block(initializeSut);
      return SyntaxFactory.MethodDeclaration(
        // return void
        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
        // method name
        SyntaxFactory.Identifier("SetUp"))
        // [SetUp] attribute
        .WithAttributeLists(
          SyntaxFactory.SingletonList(
            SyntaxFactory.AttributeList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("SetUp"))))))
        // public virtual
        .WithModifiers(
          SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
            SyntaxFactory.Token(SyntaxKind.VirtualKeyword)))
        .WithBody(setupBody);
    }
  }
}