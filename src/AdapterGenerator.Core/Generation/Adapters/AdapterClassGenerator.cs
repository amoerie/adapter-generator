using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class AdapterClassGenerator : IAdapterClassGenerator {
    private readonly IAdaptMethodGenerator _methodGenerator;
    private readonly IAdapterClassFieldsGenerator _fieldsGenerator;
    private readonly IAdapterClassConstructorGenerator _constructorGenerator;

    public AdapterClassGenerator(IAdapterClassFieldsGenerator fieldsGenerator,
      IAdapterClassConstructorGenerator constructorGenerator, IAdaptMethodGenerator methodGenerator) {
      if (methodGenerator == null) throw new ArgumentNullException(nameof(methodGenerator));
      if (fieldsGenerator == null) throw new ArgumentNullException(nameof(fieldsGenerator));
      if (constructorGenerator == null) throw new ArgumentNullException(nameof(constructorGenerator));
      _fieldsGenerator = fieldsGenerator;
      _constructorGenerator = constructorGenerator;
      _methodGenerator = methodGenerator;
    }

    public ClassDeclarationSyntax Generate(IClassAdapterGenerationContextWithNamespace context) {
      // make a new class, "BlablaAdapter"
      var classDeclaration = ClassDeclaration(context.Blueprint.Name.Identifier)
        // make it public
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        // make it implement IAdapter<TSource, TTarget>
        .WithBaseList(BaseList(SingletonSeparatedList<BaseTypeSyntax>(SimpleBaseType(context.Blueprint.InterfaceType))));
      var generationContextWithClass = context.WithClass(classDeclaration);
      return classDeclaration
        .WithMembers(List<MemberDeclarationSyntax>(_fieldsGenerator.Generate(generationContextWithClass))
          .Add(_constructorGenerator.Generate(generationContextWithClass))
          .Add(_methodGenerator.Generate(generationContextWithClass)));
    }

    public ClassDeclarationSyntax Generate(IEnumAdapterGenerationContextWithNamespace context) {
      // make a new class, "BlablaAdapter"
      var classDeclaration = ClassDeclaration(context.Blueprint.Name.Identifier)
        // make it public
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        // make it implement IAdapter<TSource, TTarget>
        .WithBaseList(BaseList(SingletonSeparatedList<BaseTypeSyntax>(SimpleBaseType(context.Blueprint.InterfaceType))));
      var generationContextWithClass = context.WithClass(classDeclaration);
      return classDeclaration
        .WithMembers(SingletonList<MemberDeclarationSyntax>(_methodGenerator.Generate(generationContextWithClass)));
    }
  }
}