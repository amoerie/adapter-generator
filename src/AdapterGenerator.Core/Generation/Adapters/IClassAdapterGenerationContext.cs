using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IClassAdapterGenerationContext {
    ITypeDeclarations Sources { get; }
    ITypeDeclarations Targets { get; }
    IImmutableList<IAdapterBlueprint> AllBlueprints { get; }
    IClassAdapterBlueprint Blueprint { get; }
    IClassAdapterGenerationContextWithCompilationUnit WithCompilationUnit(ICompilationUnitSyntax compilationUnit);
  }

  public interface IClassAdapterGenerationContextWithCompilationUnit : IClassAdapterGenerationContext {
    ICompilationUnitSyntax CompilationUnit { get; }
    IClassAdapterGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace);
  }

  public interface IClassAdapterGenerationContextWithNamespace : IClassAdapterGenerationContextWithCompilationUnit {
    NamespaceDeclarationSyntax Namespace { get; }
    IClassAdapterGenerationContextWithClass WithClass(ClassDeclarationSyntax @class);
  }

  public interface IClassAdapterGenerationContextWithClass : IClassAdapterGenerationContextWithNamespace {
    ClassDeclarationSyntax Class { get; }
    IClassAdapterGenerationContextWithMethod WithMethod(MethodDeclarationSyntax method);
  }

  public interface IClassAdapterGenerationContextWithMethod : IClassAdapterGenerationContextWithClass {
    MethodDeclarationSyntax Method { get; }
  }
}