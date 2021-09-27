using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes.Properties;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters.Properties {
  public class NestedAdapterForIEnumerableStatementsGenerator : IPropertyAdaptingStatementsGenerator {
    private readonly IClassAdapterGenerationContext _classAdapterGenerationContext;
    private readonly IPropertyMatch _propertyMatch;
    private readonly IAdapterBlueprint _nestedAdapter;
    private readonly ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;

    public NestedAdapterForIEnumerableStatementsGenerator(IClassAdapterGenerationContext classAdapterGenerationContext,
      IPropertyMatch propertyMatch, IAdapterBlueprint nestedAdapter, ITypeSyntaxAnalyzer typeSyntaxAnalyzer) {
      if (classAdapterGenerationContext == null)
        throw new ArgumentNullException(nameof(classAdapterGenerationContext));
      if (propertyMatch == null) throw new ArgumentNullException(nameof(propertyMatch));
      if (nestedAdapter == null) throw new ArgumentNullException(nameof(nestedAdapter));
      if (typeSyntaxAnalyzer == null) throw new ArgumentNullException(nameof(typeSyntaxAnalyzer));
      _classAdapterGenerationContext = classAdapterGenerationContext;
      _propertyMatch = propertyMatch;
      _nestedAdapter = nestedAdapter;
      _typeSyntaxAnalyzer = typeSyntaxAnalyzer;
    }

    public IImmutableList<StatementSyntax> Generate() {
      var targetProperty = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("target"),
        IdentifierName(_propertyMatch.Target.PropertyDeclarationSyntax.Identifier.Text));
      var sourceProperty = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("source"),
        IdentifierName(_propertyMatch.Source.PropertyDeclarationSyntax.Identifier.Text));
      var selectFromSourcePropertyAndAdaptWithNestedAdapter = 
        ConditionalAccessExpression(
          sourceProperty,
          InvocationExpression(MemberBindingExpression(IdentifierName("Select")))
        .WithArgumentList(
          ArgumentList(
            SingletonSeparatedList(
              Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(_nestedAdapter.FieldName), IdentifierName("Adapt")))))));

      var assignStatement = ExpressionStatement(
        AssignmentExpression(
          SyntaxKind.SimpleAssignmentExpression,
          targetProperty,
          ConvertIfNecessary(selectFromSourcePropertyAndAdaptWithNestedAdapter)));
      return ImmutableList.Create<StatementSyntax>(assignStatement);
    }

    private ExpressionSyntax ConvertIfNecessary(ExpressionSyntax value) {
      var targetType = _propertyMatch.Target.PropertyDeclarationSyntax.Type;
      // if target is some collection, call ToList
      if (_typeSyntaxAnalyzer.IsCollectionType(targetType))
        return InvocationExpression(
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            value,
            IdentifierName("ToList")));
      // if target is some array, call ToArray
      if (_typeSyntaxAnalyzer.IsArray(targetType))
        return InvocationExpression(
          MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            value,
            IdentifierName("ToArray")));
      return value;
    }
  }
}