using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class PropertyTestStatementsForSingleValueWithNestedAdapterGenerator : IPropertyTestStatementsGenerator {
    private readonly IClassAdapterTestsGenerationContextWithClass _context;
    private readonly IPropertyMatch _propertyMatch;
    private readonly IAdapterBlueprint _nestedAdapter;
    private readonly IDummyValueFactory _dummyValueFactory;

    public PropertyTestStatementsForSingleValueWithNestedAdapterGenerator(
      IClassAdapterTestsGenerationContextWithClass context,
      IPropertyMatch propertyMatch, IDummyValueFactory dummyValueFactory, IAdapterBlueprint nestedAdapter) {
      if (context == null) throw new ArgumentNullException(nameof(context));
      if (propertyMatch == null) throw new ArgumentNullException(nameof(propertyMatch));
      if (dummyValueFactory == null) throw new ArgumentNullException(nameof(dummyValueFactory));
      if (nestedAdapter == null) throw new ArgumentNullException(nameof(nestedAdapter));
      _context = context;
      _propertyMatch = propertyMatch;
      _dummyValueFactory = dummyValueFactory;
      _nestedAdapter = nestedAdapter;
    }

    public IImmutableList<StatementSyntax> Generate() {
      var dummySourceValue = _dummyValueFactory.CreateDummyValue(_context.Sources,
        _propertyMatch.Source.PropertyDeclarationSyntax.Type);
      var dummyTargetValue = _dummyValueFactory.CreateDummyValue(_context.Targets,
        _propertyMatch.Target.PropertyDeclarationSyntax.Type);

      // _source.SourceProperty = dummyValue;
      var sourceProperty = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("_source"),
        IdentifierName(_propertyMatch.Source.PropertyDeclarationSyntax.Identifier));
      var assignDummyValueToSourceProperty = ExpressionStatement(
        AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          sourceProperty,
          dummySourceValue));
      // var targetProperty = dummyTargetValue;
      var targetValue = Identifier($"target{_nestedAdapter.TargetType.Identifier.Text}");
      var assignDummyTargetValueToTargetProperty = LocalDeclarationStatement(
        VariableDeclaration(
          IdentifierName("var"))
          .WithVariables(
            SingletonSeparatedList<VariableDeclaratorSyntax>(
              VariableDeclarator(
                targetValue)
                .WithInitializer(EqualsValueClause(dummyTargetValue)))));

      // A.CallTo(() => _nestedAdapter.Adapt(_source.SourceProperty)).Returns(targetProperty);
      var aCallToNestedAdapterReturnsDummyTargetValue = ExpressionStatement(
        InvocationExpression(
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            InvocationExpression(
              MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("A"),
                IdentifierName("CallTo")))
              .WithArgumentList(
                ArgumentList(
                  SingletonSeparatedList(
                    Argument(
                      ParenthesizedLambdaExpression(
                        InvocationExpression(
                          MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(_nestedAdapter.FieldName),
                            IdentifierName("Adapt")))
                          .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(sourceProperty))))))))),
            IdentifierName("Returns")))
          .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName(targetValue))))));
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
            InvocationExpression(
              MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                MemberAccessExpression(
                  SyntaxKind.SimpleMemberAccessExpression,
                  IdentifierName("target"),
                  IdentifierName(_propertyMatch.Target.PropertyDeclarationSyntax.Identifier)),
                IdentifierName("Should"))),
            IdentifierName("Be")))
          .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName(targetValue))))));
      return ImmutableList.Create<StatementSyntax>(
        assignDummyValueToSourceProperty,
        assignDummyTargetValueToTargetProperty,
        aCallToNestedAdapterReturnsDummyTargetValue,
        targetIsSutAdaptSource,
        targetPropertyShouldEqualSourceProperty);
    }
  }
}