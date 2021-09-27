using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdaptMethodGenerator : IAdaptMethodGenerator {
    private readonly IAdaptMethodBodyGenerator _bodyGenerator;

    public AdaptMethodGenerator(IAdaptMethodBodyGenerator bodyGenerator) {
      if (bodyGenerator == null) throw new ArgumentNullException(nameof(bodyGenerator));
      _bodyGenerator = bodyGenerator;
    }

    public MethodDeclarationSyntax Generate(IClassAdapterGenerationContextWithClass classAdapterGenerationContext) {
      var classMatch = classAdapterGenerationContext.Blueprint.ClassMatch;
      // create the Adapt method
      return MethodDeclaration(classMatch.Target.QualifiedName, Identifier("Adapt"))
        // make it public
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        // add the "Blabla source" parameter
        .WithParameterList(ParameterList(SeparatedList(new[] {
          Parameter(Identifier("source")).WithType(classMatch.Source.QualifiedName)
        })))
        // add the body
        .WithBody(_bodyGenerator.Generate(classAdapterGenerationContext));
    }

    public MethodDeclarationSyntax Generate(IEnumAdapterGenerationContextWithClass context) {
      var enumMatch = context.Blueprint.EnumMatch;
      // create the Adapt method
      return MethodDeclaration(enumMatch.Target.QualifiedName, Identifier("Adapt"))
        // make it public
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        // add the "Blabla source" parameter
        .WithParameterList(ParameterList(SeparatedList(new[] {
          Parameter(Identifier("source")).WithType(enumMatch.Source.QualifiedName)
        })))
        // add the body
        .WithBody(_bodyGenerator.Generate(context));
    }
  }
}