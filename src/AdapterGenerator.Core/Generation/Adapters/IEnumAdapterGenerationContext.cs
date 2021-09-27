using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IEnumAdapterGenerationContext {
    ITypeDeclarations Sources { get; }
    ITypeDeclarations Targets { get; }
    IImmutableList<IAdapterBlueprint> AllBlueprints { get; }
    IEnumAdapterBlueprint Blueprint { get; }
    IEnumAdapterGenerationContextWithCompilationUnit WithCompilationUnit(ICompilationUnitSyntax compilationUnit);
  }

  public interface IEnumAdapterGenerationContextWithCompilationUnit : IEnumAdapterGenerationContext {
    ICompilationUnitSyntax CompilationUnit { get; }
    IEnumAdapterGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace);
  }

  public interface IEnumAdapterGenerationContextWithNamespace : IEnumAdapterGenerationContextWithCompilationUnit {
    NamespaceDeclarationSyntax Namespace { get; }
    IEnumAdapterGenerationContextWithClass WithClass(ClassDeclarationSyntax @class);
  }

  public interface IEnumAdapterGenerationContextWithClass : IEnumAdapterGenerationContextWithNamespace {
    ClassDeclarationSyntax Class { get; }
    IEnumAdapterGenerationContextWithMethod WithMethod(MethodDeclarationSyntax method);
  }

  public interface IEnumAdapterGenerationContextWithMethod : IEnumAdapterGenerationContextWithClass {
    MethodDeclarationSyntax Method { get; }
  }
}