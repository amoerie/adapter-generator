using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.Net;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters {
  [TestFixture]
  public class ClassGenerationContextTests {
    IClassAdapterGenerationContextWithMethod _sut;
    private ITypeDeclarations _sources;
    private ITypeDeclarations _targets;
    private IClassAdapterBlueprint _adapterBlueprint;
    private ICompilationUnitSyntax _compilationUnit;
    private NamespaceDeclarationSyntax _namespace;
    private ClassDeclarationSyntax _class;
    private MethodDeclarationSyntax _method;
    private IImmutableList<IAdapterBlueprint> _allAdapterBlueprints;

    [SetUp]
    public virtual void SetUp() {
      _sources = _sources.Fake();
      _targets = _targets.Fake();
      _allAdapterBlueprints = _allAdapterBlueprints.Fake();
      _adapterBlueprint = _adapterBlueprint.Fake();
      _compilationUnit = _compilationUnit.Fake();
      _namespace = _namespace.CreateUninitializedInstance();
      _class = _class.CreateUninitializedInstance();
      _method = _method.CreateUninitializedInstance();
      _sut = ClassAdapterGenerationContext.Create(_sources, _targets, _allAdapterBlueprints, _adapterBlueprint)
        .WithCompilationUnit(_compilationUnit)
        .WithNamespace(_namespace)
        .WithClass(_class)
        .WithMethod(_method);
    }

    [Test]
    public void ShouldHaveAllSourceClasses() {
      _sut.Sources.Should().BeSameAs(_sources);
    }

    [Test]
    public void ShouldHaveAllTargetClasses() {
      _sut.Targets.Should().BeSameAs(_targets);
    }

    [Test]
    public void ShouldHaveAllClassMatches() {
      _sut.AllBlueprints.Should().BeSameAs(_allAdapterBlueprints);
    }

    [Test]
    public void ShouldHaveClassMatch() {
      _sut.Blueprint.Should().BeSameAs(_adapterBlueprint);
    }

    [Test]
    public void ShouldHaveCompilationUnit() {
      _sut.CompilationUnit.Should().BeSameAs(_compilationUnit);
    }

    [Test]
    public void ShouldHaveNamespace() {
      _sut.Namespace.Should().BeSameAs(_namespace);
    }

    [Test]
    public void ShouldHaveClass() {
      _sut.Class.Should().BeSameAs(_class);
    }

    [Test]
    public void ShouldHaveMethod() {
      _sut.Method.Should().BeSameAs(_method);
    }
  }
}