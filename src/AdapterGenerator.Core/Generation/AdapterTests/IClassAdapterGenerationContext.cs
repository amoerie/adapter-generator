using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public interface IClassAdapterTestsGenerationContext {
    ITypeDeclarations Sources { get; }
    ITypeDeclarations Targets { get; }
    IImmutableList<IGeneratedAdapter> AllAdapters { get; }
    IGeneratedClassAdapter Adapter { get; }
    string TestClassName { get; }
    IClassAdapterTestsGenerationContextWithCompilationUnit WithCompilationUnit(ICompilationUnitSyntax compilationUnit);
  }

  public interface IClassAdapterTestsGenerationContextWithCompilationUnit : IClassAdapterTestsGenerationContext {
    ICompilationUnitSyntax CompilationUnit { get; }
    IClassAdapterTestsGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace);
  }

  public interface IClassAdapterTestsGenerationContextWithNamespace :
    IClassAdapterTestsGenerationContextWithCompilationUnit {
    NamespaceDeclarationSyntax Namespace { get; }
    IClassAdapterTestsGenerationContextWithClass WithClass(ClassDeclarationSyntax testClass);
  }

  public interface IClassAdapterTestsGenerationContextWithClass : IClassAdapterTestsGenerationContextWithNamespace {
    ClassDeclarationSyntax Class { get; }
  }
}