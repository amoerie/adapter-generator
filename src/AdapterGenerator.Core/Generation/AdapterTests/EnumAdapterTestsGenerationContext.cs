using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public class EnumAdapterTestsGenerationContext : IEnumAdapterTestsGenerationContextWithClass {
    public ITypeDeclarations Sources { get; }
    public ITypeDeclarations Targets { get; }
    public IImmutableList<IGeneratedAdapter> AllAdapters { get; }
    public IGeneratedEnumAdapter Adapter { get; }
    public string TestClassName => $"{Adapter.Blueprint.Name.Identifier.Text}Tests";
    public ICompilationUnitSyntax CompilationUnit { get; }
    public NamespaceDeclarationSyntax Namespace { get; }
    public ClassDeclarationSyntax Class { get; }

    private EnumAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedEnumAdapter adapter) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
    }

    private EnumAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedEnumAdapter adapter,
      ICompilationUnitSyntax compilationUnit) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
      CompilationUnit = compilationUnit;
    }

    private EnumAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedEnumAdapter adapter,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
      CompilationUnit = compilationUnit;
      Namespace = ns;
    }

    private EnumAdapterTestsGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedEnumAdapter adapter,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns, ClassDeclarationSyntax @class) {
      Sources = sources;
      Targets = targets;
      AllAdapters = allAdapters;
      Adapter = adapter;
      CompilationUnit = compilationUnit;
      Namespace = ns;
      Class = @class;
    }

    public static IEnumAdapterTestsGenerationContext Create(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IGeneratedAdapter> allAdapters, IGeneratedEnumAdapter adapter) {
      return new EnumAdapterTestsGenerationContext(sources, targets, allAdapters, adapter);
    }

    public IEnumAdapterTestsGenerationContextWithCompilationUnit WithCompilationUnit(
      ICompilationUnitSyntax compilationUnit) {
      return new EnumAdapterTestsGenerationContext(Sources, Targets, AllAdapters, Adapter, compilationUnit);
    }

    public IEnumAdapterTestsGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace) {
      return new EnumAdapterTestsGenerationContext(Sources, Targets, AllAdapters, Adapter, CompilationUnit, @namespace);
    }

    public IEnumAdapterTestsGenerationContextWithClass WithClass(ClassDeclarationSyntax @class) {
      return new EnumAdapterTestsGenerationContext(Sources, Targets, AllAdapters, Adapter, CompilationUnit, Namespace,
        @class);
    }
  }
}