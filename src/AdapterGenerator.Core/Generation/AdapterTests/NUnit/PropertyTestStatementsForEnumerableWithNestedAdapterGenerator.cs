using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class PropertyTestStatementsForEnumerableWithNestedAdapterGenerator : IPropertyTestStatementsGenerator {
    private readonly IClassAdapterTestsGenerationContextWithClass _context;
    private readonly IPropertyMatch _propertyMatch;
    private readonly IAdapterBlueprint _nestedAdapter;
    private readonly IDummyValueFactory _dummyValueFactory;

    public PropertyTestStatementsForEnumerableWithNestedAdapterGenerator(
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
      // _source.SourceProperty = dummySourceValue;
      var sourceProperty = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("_source"),
        IdentifierName(_propertyMatch.Source.PropertyDeclarationSyntax.Identifier));
      var assignDummyValueToSourceProperty = ExpressionStatement(
        AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          sourceProperty,
          dummySourceValue));
      // var expected = dummyTargetValue;
      var expectedIsDummyTargetValue = LocalDeclarationStatement(VariableDeclaration(IdentifierName("var"))
        .WithVariables(
          SingletonSeparatedList(
            VariableDeclarator(Identifier("expected")).WithInitializer(EqualsValueClause(dummyTargetValue)))));
      // A.CallTo(() => _nestedAdapter.Adapt(_source.Property.ElementAt(0)).Returns(expected.ElementAt(0));
      // A.CallTo(() => _nestedAdapter.Adapt(_source.Property.ElementAt(1)).Returns(expected.ElementAt(1));
      // A.CallTo(() => _nestedAdapter.Adapt(_source.Property.ElementAt(2)).Returns(expected.ElementAt(2));
      var firstThreeSourcePropertyValues = new[] {0, 1, 2}.Select(i => InvocationExpression(
            MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              sourceProperty,
              IdentifierName("ElementAt")))
          .WithArgumentList(
            ArgumentList(
              SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(i)))))))
        .ToImmutableList();
      var firstThreeExpectedPropertyValues = new[] {0, 1, 2}.Select(i => InvocationExpression(
            MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              IdentifierName("expected"),
              IdentifierName("ElementAt")))
          .WithArgumentList(
            ArgumentList(
              SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(i)))))))
        .ToImmutableList();
      var callsToNestedAdapter = firstThreeSourcePropertyValues.Zip(firstThreeExpectedPropertyValues,
        (sourcePropertyValue, expectedValue) =>
          ExpressionStatement(
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
                                .WithArgumentList(
                                  ArgumentList(SingletonSeparatedList(Argument(sourcePropertyValue))))))))),
                  IdentifierName("Returns")))
              .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(expectedValue))))));
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

      // target.Property.Should().HaveCount(3);
      var targetProperty = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("target"),
        IdentifierName(_propertyMatch.Target.PropertyDeclarationSyntax.Identifier));

      var targetPropertyCountShouldBeThree = ExpressionStatement(
        InvocationExpression(
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            InvocationExpression(
              MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                targetProperty,
                IdentifierName("Should")
              )
            ),
            IdentifierName("HaveCount")
          )
        ).WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(3)))))));

      // target.Property.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
      // target.Property.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
      // target.Property.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
      var firstThreeTargetPropertyValues = new[] {0, 1, 2}.Select(i => InvocationExpression(
            MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              targetProperty,
              IdentifierName("ElementAt")))
          .WithArgumentList(
            ArgumentList(
              SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(i)))))))
        .ToImmutableList();
      var firstThreeTargetValuesShouldBeSameAsFirstThreeExpectedValues = firstThreeTargetPropertyValues
        .Zip(firstThreeExpectedPropertyValues, (targetPropertyValue, expectedValue) =>
            ExpressionStatement(InvocationExpression(
              MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                InvocationExpression(
                  MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, targetPropertyValue,
                    IdentifierName("Should"))
                ),
                IdentifierName("BeSameAs")
              )
            ).WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(expectedValue)))))
        );
      return ImmutableList.Create<StatementSyntax>(assignDummyValueToSourceProperty)
        .Add(expectedIsDummyTargetValue)
        .AddRange(callsToNestedAdapter)
        .Add(targetIsSutAdaptSource)
        .Add(targetPropertyCountShouldBeThree)
        .AddRange(firstThreeTargetValuesShouldBeSameAsFirstThreeExpectedValues);
    }
  }
}