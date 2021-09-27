using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.Blueprints {
  [TestFixture]
  public class AdaptersBlueprintFactoryTests {
    AdaptersBlueprintFactory _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new AdaptersBlueprintFactory(new TypeSyntaxAnalyzer(), new ConsoleLogger());
    }

    [TestFixture]
    public class Constructor : AdaptersBlueprintFactoryTests {
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
    public class CreateBlueprints : AdaptersBlueprintFactoryTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldCreateCorrectBluePrintsForSimpleMatch() {
        var sourceClass = TestUtilities.ParseClass("public class Source {}");
        var targetClass = TestUtilities.ParseClass("public class Target {}");
        var match = new ClassMatch {
          Source = sourceClass,
          Target = targetClass,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        var matches = new Matches(ImmutableList.Create<IClassMatch>(match), ImmutableList<IEnumMatch>.Empty);
        var blueprints = _sut.CreateBlueprints(matches);
        blueprints.Should().HaveCount(1);
        var blueprint = blueprints.OfType<ClassAdapterBlueprint>().Single();
        blueprint.ClassMatch.Should().Be(match);
        blueprint.FieldName.Should().Be("_targetAdapter");
        blueprint.Name.Identifier.Text.Should().Be("TargetAdapter");
        blueprint.ParameterName.Should().Be("targetAdapter");
        blueprint.NestedAdapters.Should().BeEmpty();
      }

      [Test]
      public void ShouldCreateCorrectBluePrintsForNestedMatch() {
        var nestedSourceClass = TestUtilities.ParseClass("public class NestedSource {  }");
        var nestedTargetClass = TestUtilities.ParseClass("public class NestedTarget {  }");
        var sourceClass = TestUtilities.ParseClass("public class Source { public NestedSource Nested { get; set; } }");
        var targetClass = TestUtilities.ParseClass("public class Target { public NestedTarget Nested { get; set; } }");
        var mainMatch = new ClassMatch {
          Source = sourceClass,
          Target = targetClass,
          PropertyMatches = ImmutableList.Create<IPropertyMatch>(new PropertyMatch {
            Source = sourceClass.FindPropertyByName("Nested"),
            Target = targetClass.FindPropertyByName("Nested"),
          })
        };
        var nestedMatch = new ClassMatch {
          Source = nestedSourceClass,
          Target = nestedTargetClass,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        var matches = new Matches(ImmutableList.Create<IClassMatch>(mainMatch, nestedMatch),
          ImmutableList<IEnumMatch>.Empty);
        var blueprints = _sut.CreateBlueprints(matches);
        blueprints.Should().HaveCount(2);
        var mainBlueprint = blueprints.OfType<ClassAdapterBlueprint>().First();
        mainBlueprint.ClassMatch.Should().Be(mainMatch);
        mainBlueprint.FieldName.Should().Be("_targetAdapter");
        mainBlueprint.Name.Identifier.Text.Should().Be("TargetAdapter");
        mainBlueprint.ParameterName.Should().Be("targetAdapter");
        mainBlueprint.NestedAdapters.Should().NotBeEmpty();
        var nestedAdapter = mainBlueprint.NestedAdapters.OfType<ClassAdapterBlueprint>().Single();
        nestedAdapter.ClassMatch.Should().Be(nestedMatch);
        nestedAdapter.Name.Identifier.Text.Should().Be("NestedTargetAdapter");
      }

      [Test]
      public void ShouldCreateCorrectBluePrintsForEnumerableNestedMatch() {
        var nestedSourceClass = TestUtilities.ParseClass("public class NestedSource {  }");
        var nestedTargetClass = TestUtilities.ParseClass("public class NestedTarget {  }");
        var sourceClass =
          TestUtilities.ParseClass("public class Source { public IEnumerable<NestedSource> Nested { get; set; } }");
        var targetClass =
          TestUtilities.ParseClass("public class Target { public IEnumerable<NestedTarget> Nested { get; set; } }");
        var mainMatch = new ClassMatch {
          Source = sourceClass,
          Target = targetClass,
          PropertyMatches = ImmutableList.Create<IPropertyMatch>(new PropertyMatch {
            Source = sourceClass.FindPropertyByName("Nested"),
            Target = targetClass.FindPropertyByName("Nested"),
          })
        };
        var nestedMatch = new ClassMatch {
          Source = nestedSourceClass,
          Target = nestedTargetClass,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        var matches = new Matches(ImmutableList.Create<IClassMatch>(mainMatch, nestedMatch),
          ImmutableList<IEnumMatch>.Empty);
        var blueprints = _sut.CreateBlueprints(matches);
        blueprints.Should().HaveCount(2);
        var mainBlueprint = blueprints.OfType<ClassAdapterBlueprint>().First();
        mainBlueprint.ClassMatch.Should().Be(mainMatch);
        mainBlueprint.FieldName.Should().Be("_targetAdapter");
        mainBlueprint.Name.Identifier.Text.Should().Be("TargetAdapter");
        mainBlueprint.ParameterName.Should().Be("targetAdapter");
        mainBlueprint.NestedAdapters.Should().NotBeEmpty();
        var nestedAdapter = mainBlueprint.NestedAdapters.OfType<ClassAdapterBlueprint>().Single();
        nestedAdapter.ClassMatch.Should().Be(nestedMatch);
        nestedAdapter.Name.Identifier.Text.Should().Be("NestedTargetAdapter");
      }

      [Test]
      public void ShouldCreateCorrectBluePrintsForNestedArrayMatch() {
        var nestedSourceClass = TestUtilities.ParseClass("public class NestedSource {  }");
        var nestedTargetClass = TestUtilities.ParseClass("public class NestedTarget {  }");
        var sourceClass = TestUtilities.ParseClass("public class Source { public NestedSource[] Nested { get; set; } }");
        var targetClass = TestUtilities.ParseClass("public class Target { public NestedTarget[] Nested { get; set; } }");
        var mainMatch = new ClassMatch {
          Source = sourceClass,
          Target = targetClass,
          PropertyMatches = ImmutableList.Create<IPropertyMatch>(new PropertyMatch {
            Source = sourceClass.FindPropertyByName("Nested"),
            Target = targetClass.FindPropertyByName("Nested"),
          })
        };
        var nestedMatch = new ClassMatch {
          Source = nestedSourceClass,
          Target = nestedTargetClass,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        var matches = new Matches(ImmutableList.Create<IClassMatch>(mainMatch, nestedMatch),
          ImmutableList<IEnumMatch>.Empty);
        var blueprints = _sut.CreateBlueprints(matches);
        blueprints.Should().HaveCount(2);
        var mainBlueprint = blueprints.OfType<ClassAdapterBlueprint>().First();
        mainBlueprint.FieldName.Should().Be("_targetAdapter");
        mainBlueprint.Name.Identifier.Text.Should().Be("TargetAdapter");
        mainBlueprint.ParameterName.Should().Be("targetAdapter");
        mainBlueprint.NestedAdapters.Should().NotBeEmpty();
        var nestedAdapter = mainBlueprint.NestedAdapters.OfType<ClassAdapterBlueprint>().Single();
        nestedAdapter.ClassMatch.Should().Be(nestedMatch);
        nestedAdapter.Name.Identifier.Text.Should().Be("NestedTargetAdapter");
      }
    }
  }
}