using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class ClassAdapterGenerationContext : IClassAdapterGenerationContextWithMethod {
    public ITypeDeclarations Sources { get; }
    public ITypeDeclarations Targets { get; }
    public IImmutableList<IAdapterBlueprint> AllBlueprints { get; }
    public IClassAdapterBlueprint Blueprint { get; }
    public ICompilationUnitSyntax CompilationUnit { get; }
    public NamespaceDeclarationSyntax Namespace { get; }
    public ClassDeclarationSyntax Class { get; }
    public MethodDeclarationSyntax Method { get; }

    private ClassAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IClassAdapterBlueprint blueprint) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
    }

    private ClassAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IClassAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
    }

    private ClassAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IClassAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
      Namespace = ns;
    }

    private ClassAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IClassAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns, ClassDeclarationSyntax @class) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
      Namespace = ns;
      Class = @class;
    }

    private ClassAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IClassAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns, ClassDeclarationSyntax @class,
      MethodDeclarationSyntax method) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
      Namespace = ns;
      Class = @class;
      Method = method;
    }


    public static IClassAdapterGenerationContext Create(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBluePrints, IClassAdapterBlueprint blueprint) {
      if (sources == null) throw new ArgumentNullException(nameof(sources));
      if (targets == null) throw new ArgumentNullException(nameof(targets));
      if (allBluePrints == null) throw new ArgumentNullException(nameof(allBluePrints));
      if (blueprint == null) throw new ArgumentNullException(nameof(blueprint));
      return new ClassAdapterGenerationContext(sources, targets, allBluePrints, blueprint);
    }

    public IClassAdapterGenerationContextWithCompilationUnit WithCompilationUnit(ICompilationUnitSyntax compilationUnit) {
      return new ClassAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, compilationUnit);
    }

    public IClassAdapterGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace) {
      return new ClassAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, CompilationUnit, @namespace);
    }

    public IClassAdapterGenerationContextWithClass WithClass(ClassDeclarationSyntax @class) {
      return new ClassAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, CompilationUnit, Namespace,
        @class);
    }

    public IClassAdapterGenerationContextWithMethod WithMethod(MethodDeclarationSyntax method) {
      return new ClassAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, CompilationUnit, Namespace,
        Class, method);
    }
  }
}