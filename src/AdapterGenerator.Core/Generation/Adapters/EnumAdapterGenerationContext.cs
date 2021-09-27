using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public class EnumAdapterGenerationContext : IEnumAdapterGenerationContextWithMethod {
    public ITypeDeclarations Sources { get; }
    public ITypeDeclarations Targets { get; }
    public IImmutableList<IAdapterBlueprint> AllBlueprints { get; }
    public IEnumAdapterBlueprint Blueprint { get; }
    public ICompilationUnitSyntax CompilationUnit { get; }
    public NamespaceDeclarationSyntax Namespace { get; }
    public ClassDeclarationSyntax Class { get; }
    public MethodDeclarationSyntax Method { get; }

    private EnumAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IEnumAdapterBlueprint blueprint) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
    }

    private EnumAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IEnumAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
    }

    private EnumAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IEnumAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
      Namespace = ns;
    }

    private EnumAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IEnumAdapterBlueprint blueprint,
      ICompilationUnitSyntax compilationUnit, NamespaceDeclarationSyntax ns, ClassDeclarationSyntax @class) {
      Sources = sources;
      Targets = targets;
      AllBlueprints = allBlueprints;
      Blueprint = blueprint;
      CompilationUnit = compilationUnit;
      Namespace = ns;
      Class = @class;
    }

    private EnumAdapterGenerationContext(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBlueprints, IEnumAdapterBlueprint blueprint,
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


    public static IEnumAdapterGenerationContext Create(ITypeDeclarations sources, ITypeDeclarations targets,
      IImmutableList<IAdapterBlueprint> allBluePrints, IEnumAdapterBlueprint blueprint) {
      if (sources == null) throw new ArgumentNullException(nameof(sources));
      if (targets == null) throw new ArgumentNullException(nameof(targets));
      if (allBluePrints == null) throw new ArgumentNullException(nameof(allBluePrints));
      if (blueprint == null) throw new ArgumentNullException(nameof(blueprint));
      return new EnumAdapterGenerationContext(sources, targets, allBluePrints, blueprint);
    }

    public IEnumAdapterGenerationContextWithCompilationUnit WithCompilationUnit(ICompilationUnitSyntax compilationUnit) {
      return new EnumAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, compilationUnit);
    }

    public IEnumAdapterGenerationContextWithNamespace WithNamespace(NamespaceDeclarationSyntax @namespace) {
      return new EnumAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, CompilationUnit, @namespace);
    }

    public IEnumAdapterGenerationContextWithClass WithClass(ClassDeclarationSyntax @class) {
      return new EnumAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, CompilationUnit, Namespace,
        @class);
    }

    public IEnumAdapterGenerationContextWithMethod WithMethod(MethodDeclarationSyntax method) {
      return new EnumAdapterGenerationContext(Sources, Targets, AllBlueprints, Blueprint, CompilationUnit, Namespace,
        Class, method);
    }
  }
}