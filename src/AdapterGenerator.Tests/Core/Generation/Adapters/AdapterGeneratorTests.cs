using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters {
  [TestFixture]
  public class AdapterGeneratorTests {
    global::AdapterGenerator.Core.Generation.Adapters.AdaptersGenerator _sut;
    IAdapterCompilationUnitGenerator _compilationUnitGenerator;
    IAdaptersBlueprintFactory _blueprintFactory;

    [SetUp]
    public virtual void SetUp() {
      _compilationUnitGenerator = _compilationUnitGenerator.Fake();
      _blueprintFactory = _blueprintFactory.Fake();
      _sut = new global::AdapterGenerator.Core.Generation.Adapters.AdaptersGenerator(_blueprintFactory,
        _compilationUnitGenerator, new ConsoleLogger());
    }

    [TestFixture]
    public class Constructor : AdapterGeneratorTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldHaveNoOptionalDependencies() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class GenerateAdapter : AdapterGeneratorTests {
      private ITypeDeclarations _sources;
      private ITypeDeclarations _targets;
      private IMatches _matches;
      private IImmutableList<IEnumMatch> _enumMatches;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _sources = _sources.Fake();
        _targets = _targets.Fake();
        _matches = _matches.Fake();
        _enumMatches = _enumMatches.Fake();
      }

      [Test]
      public void ShouldGenerateClassAdapters() {
        var classBlueprint = A.Fake<IClassAdapterBlueprint>();
        A.CallTo(() => classBlueprint.Name).Returns(SyntaxFactory.IdentifierName("FakeAdapter"));
        var blueprints = ImmutableList.Create<IAdapterBlueprint>(classBlueprint);
        A.CallTo(() => _blueprintFactory.CreateBlueprints(_matches)).Returns(blueprints);
        var context = ClassAdapterGenerationContext.Create(_sources, _targets, blueprints, classBlueprint);
        var compilationUnitSyntax = SyntaxFactory.CompilationUnit();
        A.CallTo(
          () =>
            _compilationUnitGenerator.Generate(
              (IClassAdapterGenerationContext) A<IClassAdapterGenerationContext>.That.HasSamePropertyValuesAs(context)))
          .Returns(compilationUnitSyntax);
        var generatedAdapters = _sut.Generate(_sources, _targets, _matches);
        generatedAdapters.Should().HaveCount(1);
        var generatedAdapter = generatedAdapters.Single();
        generatedAdapter.Blueprint.Should().Be(classBlueprint);
        generatedAdapter.CompilationUnitSyntax.Should().Be(compilationUnitSyntax);
      }

      [Test]
      public void ShouldGenerateEnumAdapters() {
        var enumBlueprint = A.Fake<IEnumAdapterBlueprint>();
        A.CallTo(() => enumBlueprint.Name).Returns(SyntaxFactory.IdentifierName("FakeAdapter"));
        var blueprints = ImmutableList.Create<IAdapterBlueprint>(enumBlueprint);
        A.CallTo(() => _blueprintFactory.CreateBlueprints(_matches)).Returns(blueprints);
        var context = EnumAdapterGenerationContext.Create(_sources, _targets, blueprints, enumBlueprint);
        var compilationUnitSyntax = SyntaxFactory.CompilationUnit();
        A.CallTo(
          () =>
            _compilationUnitGenerator.Generate(
              (IEnumAdapterGenerationContext) A<IEnumAdapterGenerationContext>.That.HasSamePropertyValuesAs(context)))
          .Returns(compilationUnitSyntax);
        var generatedAdapters = _sut.Generate(_sources, _targets, _matches);
        generatedAdapters.Should().HaveCount(1);
        var generatedAdapter = generatedAdapters.Single();
        generatedAdapter.Blueprint.Should().Be(enumBlueprint);
        generatedAdapter.CompilationUnitSyntax.Should().Be(compilationUnitSyntax);
      }
    }
  }
}