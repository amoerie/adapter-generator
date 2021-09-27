using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsClassGenerator : IAdapterTestsClassGenerator {
    private readonly IAdapterTestsClassFieldsGenerator _fieldsGenerator;
    private readonly IAdapterTestsSetupMethodGenerator _setupGenerator;
    private readonly IAdapterTestsConstructorTestsGenerator _constructorTestsGenerator;
    private readonly IAdapterTestsAdaptMethodTestsGenerator _adaptMethodTestsGenerator;

    public AdapterTestsClassGenerator(IAdapterTestsClassFieldsGenerator fieldsGenerator,
      IAdapterTestsSetupMethodGenerator setupGenerator,
      IAdapterTestsConstructorTestsGenerator constructorTestsGenerator,
      IAdapterTestsAdaptMethodTestsGenerator adaptMethodTestsGenerator) {
      if (fieldsGenerator == null) throw new ArgumentNullException(nameof(fieldsGenerator));
      if (setupGenerator == null) throw new ArgumentNullException(nameof(setupGenerator));
      if (constructorTestsGenerator == null) throw new ArgumentNullException(nameof(constructorTestsGenerator));
      if (adaptMethodTestsGenerator == null) throw new ArgumentNullException(nameof(adaptMethodTestsGenerator));
      _fieldsGenerator = fieldsGenerator;
      _setupGenerator = setupGenerator;
      _constructorTestsGenerator = constructorTestsGenerator;
      _adaptMethodTestsGenerator = adaptMethodTestsGenerator;
    }

    public ClassDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithNamespace context) {
      // make a new class, "BlablaAdapter"
      var classDeclaration = SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(context.TestClassName))
        // make it public
        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
        // add [TestFixture] attribute
        .WithAttributeLists(
          SyntaxFactory.SingletonList(
            SyntaxFactory.AttributeList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("TestFixture"))))));
      var contextWithClass = context.WithClass(classDeclaration);
      return classDeclaration
        .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(_fieldsGenerator.Generate(contextWithClass))
          .Add(_setupGenerator.Generate(contextWithClass))
          .Add(_constructorTestsGenerator.Generate(contextWithClass))
          .Add(_adaptMethodTestsGenerator.Generate(contextWithClass)));
    }

    public ClassDeclarationSyntax Generate(IEnumAdapterTestsGenerationContextWithNamespace context) {
      // make a new class, "BlablaAdapter"
      var classDeclaration = SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(context.TestClassName))
        // make it public
        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
        // add [TestFixture] attribute
        .WithAttributeLists(
          SyntaxFactory.SingletonList(
            SyntaxFactory.AttributeList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("TestFixture"))))));
      var contextWithClass = context.WithClass(classDeclaration);
      return classDeclaration
        .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(_fieldsGenerator.Generate(contextWithClass))
          .Add(_setupGenerator.Generate(contextWithClass))
          .Add(_constructorTestsGenerator.Generate(contextWithClass))
          .Add(_adaptMethodTestsGenerator.Generate(contextWithClass)));
    }
  }
}