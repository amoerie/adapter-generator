using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public interface IEnumAdapterTestsGenerationContext {
    ITypeDeclarations Sources { get; }
    ITypeDeclarations Targets { get; }
    IImmutableList<IGeneratedAdapter> AllAdapters { get; }
    IGeneratedEnumAdapter Adapter { get; }
    string TestClassName { get; }
    IEnumAdapterTestsGenerationContextWithCompilationUnit WithCompilationUnit(ICompilationUnitSyntax compilationUnit);
  }

  public interface IEnumAdapterTestsGenerationContextWithCompilationUnit : IEnumAdapterTestsGenerationContext {
    ICompilationUnitSyntax CompilationUnit { get; }
    IEnumAdapterTestsGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace);
  }

  public interface IEnumAdapterTestsGenerationContextWithNamespace :
    IEnumAdapterTestsGenerationContextWithCompilationUnit {
    NamespaceDeclarationSyntax Namespace { get; }
    IEnumAdapterTestsGenerationContextWithClass WithClass(ClassDeclarationSyntax testClass);
  }

  public interface IEnumAdapterTestsGenerationContextWithClass : IEnumAdapterTestsGenerationContextWithNamespace {
    ClassDeclarationSyntax Class { get; }
  }
}