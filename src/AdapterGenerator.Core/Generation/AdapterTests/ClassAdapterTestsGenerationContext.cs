using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public class ClassAdapterTestsGenerationContext : IClassAdapterTestsGenerationContextWithClass {
    public ITypeDeclarations Sources { get; }
    public ITypeDeclarations Targets { get; }
    public IImmutableList<IGeneratedAdapter> AllAdapters { get; }
    public IGeneratedClassAdapter Adapter { get; }
    public string TestClassName => $"{Adapter.Blueprint.Name.Identifier.Text}Tests";
    public ICompilationUnitSyntax CompilationUnit { get; }
    public NamespaceDeclarationSyntax Namespace { get; }
    public ClassDeclarationSyntax Class { get; }

    private ClassAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedClassAdapter adapter) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
    }

    private ClassAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedClassAdapter adapter,
      ICompilationUnitSyntax compilationUnit) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
      CompilationUnit = compilationUnit;
    }

    private ClassAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedClassAdapter adapter,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
      CompilationUnit = compilationUnit;
      Namespace = ns;
    }

    private ClassAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedClassAdapter adapter,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns, ClassDeclarationSyntax @class) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
      CompilationUnit = compilationUnit;
      Namespace = ns;
      Class = @class;
    }

    public static IClassAdapterTestsGenerationContext Create(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedClassAdapter adapter) {
      return new ClassAdapterTestsGenerationContext(sources, targets, allAdapters, adapter);
    }

    public IClassAdapterTestsGenerationContextWithCompilationUnit WithCompilationUnit(
      ICompilationUnitSyntax compilationUnit) {
      return new ClassAdapterTestsGenerationContext(Sources, Targets, AllAdapters, Adapter, compilationUnit);
    }

    public IClassAdapterTestsGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace) {
      return new ClassAdapterTestsGenerationContext(Sources, Targets, AllAdapters, Adapter, CompilationUnit, @namespace);
    }

    public IClassAdapterTestsGenerationContextWithClass WithClass(ClassDeclarationSyntax @class) {
      return new ClassAdapterTestsGenerationContext(Sources, Targets, AllAdapters, Adapter, CompilationUnit, Namespace,
        @class);
    }
  }
}