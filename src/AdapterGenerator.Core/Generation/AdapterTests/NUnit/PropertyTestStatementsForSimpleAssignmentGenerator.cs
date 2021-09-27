using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class PropertyTestStatementsForSimpleAssignmentGenerator : IPropertyTestStatementsGenerator {
    private readonly IClassAdapterTestsGenerationContextWithClass _context;
    private readonly IPropertyMatch _propertyMatch;
    private readonly IDummyValueFactory _dummyValueFactory;

    public PropertyTestStatementsForSimpleAssignmentGenerator(
      IClassAdapterTestsGenerationContextWithClass context,
      IPropertyMatch propertyMatch, IDummyValueFactory dummyValueFactory) {
      if (context == null) throw new ArgumentNullException(nameof(context));
      if (propertyMatch == null) throw new ArgumentNullException(nameof(propertyMatch));
      _context = context;
      _propertyMatch = propertyMatch;
      _dummyValueFactory = dummyValueFactory;
    }

    public IImmutableList<StatementSyntax> Generate() {
      // _source.SourceProperty = dummyValue;
      var dummyValue = _dummyValueFactory.CreateDummyValue(_context.Sources,
        _propertyMatch.Source.PropertyDeclarationSyntax.Type);
      var sourceProperty = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("_source"),
        IdentifierName(_propertyMatch.Source.PropertyDeclarationSyntax.Identifier));
      var assignDummyValueToSourceProperty = ExpressionStatement(
        AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          sourceProperty,
          dummyValue));
      // var target = _sut.Adapt(_source);
      var targetIsSutAdaptSource = LocalDeclarationStatement(
        VariableDeclaration(IdentifierName("var"))
          .WithVariables(
            SingletonSeparatedList(
              VariableDeclarator(Identifier("target"))
                .WithInitializer(
                  EqualsValueClause(InvocationExpression(MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("_sut"),
                    IdentifierName("Adapt")))
                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName("_source"))))))))));
      // target.Property.ShouldBeEquivalentTo(_source.Property);
      var targetPropertyShouldEqualSourceProperty = ExpressionStatement(
        InvocationExpression(
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              IdentifierName("target"),
              IdentifierName(_propertyMatch.Target.PropertyDeclarationSyntax.Identifier)),
            IdentifierName("ShouldBeEquivalentTo")))
          .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(sourceProperty)))));
      return ImmutableList.Create<StatementSyntax>(
        assignDummyValueToSourceProperty,
        targetIsSutAdaptSource,
        targetPropertyShouldEqualSourceProperty);
    }
  }
}