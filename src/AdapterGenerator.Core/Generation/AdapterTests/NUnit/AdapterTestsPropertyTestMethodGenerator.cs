using System;
using System.Linq;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsPropertyTestMethodGenerator : IAdapterTestsPropertyTestMethodGenerator {
    private readonly IAdapterTestsPropertySpecificTestStatementsGenerator _propertySpecificTestStatementsGenerator;

    public AdapterTestsPropertyTestMethodGenerator(
      IAdapterTestsPropertySpecificTestStatementsGenerator propertySpecificTestStatementsGenerator) {
      if (propertySpecificTestStatementsGenerator == null)
        throw new ArgumentNullException(nameof(propertySpecificTestStatementsGenerator));
      _propertySpecificTestStatementsGenerator = propertySpecificTestStatementsGenerator;
    }

    public MethodDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context,
      IProperty targetProperty) {
      return MethodDeclaration(
        PredefinedType(Token(SyntaxKind.VoidKeyword)),
        Identifier($"ShouldAdapt{targetProperty.PropertyDeclarationSyntax.Identifier.Text}"))
        .WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("Test"))))))
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        .WithBody(Block(_propertySpecificTestStatementsGenerator.Generate(context, targetProperty).ToArray()));
    }
  }
}