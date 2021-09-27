using System.Collections.Immutable;
using AdapterGenerator.Core.Generation;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation {
  [TestFixture]
  public class PropertyMatchAnalyzerTests {
    PropertyMatchAnalyzer _sut;
    private IClassAdapterGenerationContextWithClass _classAdapterGenerationContext;
    private IClassAdapterBlueprint _blueprint;

    [SetUp]
    public virtual void SetUp() {
      _classAdapterGenerationContext = _classAdapterGenerationContext.Fake();
      _blueprint = _classAdapterGenerationContext.Blueprint;
      _sut = new PropertyMatchAnalyzer(_blueprint,
        new TypeSyntaxComparer(_classAdapterGenerationContext.Sources, _classAdapterGenerationContext.Targets),
        new TypeSyntaxAnalyzer());
    }

    [TestFixture]
    public class Constructor : PropertyMatchAnalyzerTests {
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
    public class CanBeAdaptedThroughSimpleAssignment : PropertyMatchAnalyzerTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [TestCase("int", "int", true)]
      [TestCase("int", "int?", true)]
      [TestCase("int?", "int?", true)]
      [TestCase("int?", "int", false)]
      [TestCase("int[]", "int[]", true)]
      [TestCase("int[]", "int?[]", false)]
      [TestCase("int?[]", "int?[]", true)]
      [TestCase("int?[]", "int[]", false)]
      [TestCase("string", "string", true)]
      [TestCase("DateTime", "DateTime", true)]
      [TestCase("DateTime[]", "DateTime[]", true)]
      [TestCase("DateTime?[]", "DateTime[]", false)]
      [TestCase("DateTime[]", "DateTime?[]", false)]
      [TestCase("DateTime?[]", "DateTime?[]", true)]
      [TestCase("Nested", "Nested", false)]
      [TestCase("Nested[]", "Nested[]", false)]
      [TestCase("IEnumerable<Nested>", "IEnumerable<Nested>", false)]
      [TestCase("IEnumerable<Nested>", "ICollection<Nested>", false)]
      [TestCase("ICollection<Nested>", "IEnumerable<Nested>", false)]
      [TestCase("IEnumerable<int>", "IEnumerable<int>", true)]
      [TestCase("IEnumerable<DateTime>", "IEnumerable<DateTime>", true)]
      public void ShouldReturn(string sourceType, string targetType, bool expectedResult) {
        var nestedClass = TestUtilities.ParseClass($"public class Nested {{ public int Id {{ get; set; }} }}");
        var sourceClass =
          TestUtilities.ParseClass($"public class Source {{ public {sourceType} SourceProp {{ get; set; }} }}");
        var targetClass =
          TestUtilities.ParseClass($"public class Target {{ public {targetType} TargetProp {{ get; set; }} }}");
        var sourceProp = sourceClass.FindPropertyByName("SourceProp");
        var targetProp = targetClass.FindPropertyByName("TargetProp");
        A.CallTo(() => _classAdapterGenerationContext.Sources.Classes)
          .Returns(ImmutableList.Create(nestedClass, sourceClass));
        A.CallTo(() => _classAdapterGenerationContext.Targets.Classes)
          .Returns(ImmutableList.Create(nestedClass, targetClass));
        var propertyMatch = new PropertyMatch {Source = sourceProp, Target = targetProp};
        _sut.CanBeAdaptedThroughSimpleAssignment(propertyMatch)
          .Should()
          .Be(expectedResult,
            $"because mapping from {sourceType} to {targetType} should{(expectedResult ? "" : " not")} be possible with a simple assignment statement");
      }
    }

    [TestFixture]
    public class FindNestedAdapterForSimpleAssignment : PropertyMatchAnalyzerTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [TestCase("int", "int", false)]
      [TestCase("int", "int?", false)]
      [TestCase("int?", "int?", false)]
      [TestCase("int?", "int", false)]
      [TestCase("int[]", "int[]", false)]
      [TestCase("int[]", "int?[]", false)]
      [TestCase("int?[]", "int?[]", false)]
      [TestCase("int?[]", "int[]", false)]
      [TestCase("string", "string", false)]
      [TestCase("DateTime", "DateTime", false)]
      [TestCase("DateTime[]", "DateTime[]", false)]
      [TestCase("DateTime?[]", "DateTime[]", false)]
      [TestCase("DateTime[]", "DateTime?[]", false)]
      [TestCase("DateTime?[]", "DateTime?[]", false)]
      [TestCase("Nested", "Nested", true)]
      [TestCase("Nested[]", "Nested[]", false)]
      [TestCase("IEnumerable<Nested>", "IEnumerable<Nested>", false)]
      [TestCase("IEnumerable<Nested>", "ICollection<Nested>", false)]
      [TestCase("ICollection<Nested>", "IEnumerable<Nested>", false)]
      [TestCase("IEnumerable<int>", "IEnumerable<int>", false)]
      [TestCase("IEnumerable<DateTime>", "IEnumerable<DateTime>", false)]
      public void ShouldReturn(string sourceType, string targetType, bool expectedResult) {
        var nestedSourceClass = TestUtilities.ParseClass($"public class Nested {{ public int Id {{ get; set; }} }}");
        var nestedTargetClass = TestUtilities.ParseClass($"public class Nested {{ public int Id {{ get; set; }} }}");
        var sourceClass =
          TestUtilities.ParseClass($"public class Source {{ public {sourceType} SourceProp {{ get; set; }} }}");
        var targetClass =
          TestUtilities.ParseClass($"public class Target {{ public {targetType} TargetProp {{ get; set; }} }}");
        var sourceProp = sourceClass.FindPropertyByName("SourceProp");
        var targetProp = targetClass.FindPropertyByName("TargetProp");
        var propertyMatch = new PropertyMatch {Source = sourceProp, Target = targetProp};
        A.CallTo(() => _classAdapterGenerationContext.Sources.Classes)
          .Returns(ImmutableList.Create(nestedSourceClass, sourceClass));
        A.CallTo(() => _classAdapterGenerationContext.Targets.Classes)
          .Returns(ImmutableList.Create(nestedTargetClass, targetClass));
        var nestedClassMatch = new ClassMatch {
          Source = nestedSourceClass,
          Target = nestedTargetClass,
          PropertyMatches = ImmutableList.Create<IPropertyMatch>(),
        };
        var nestedAdapter = new ClassAdapterBlueprint(nestedClassMatch, ImmutableList<IAdapterBlueprint>.Empty);
        A.CallTo(() => _blueprint.NestedAdapters).Returns(ImmutableList.Create<IAdapterBlueprint>(nestedAdapter));
        _sut.FindNestedAdapterForSimpleAssignment(propertyMatch)
          .Should()
          .Be(expectedResult ? nestedAdapter : null,
            $"because mapping from {sourceType} to {targetType} should{(expectedResult ? "" : " not")} be possible with a nested adapter");
      }
    }

    [TestFixture]
    public class FindNestedAdapterForEnumerableAssigmnent : PropertyMatchAnalyzerTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [TestCase("int", "int", false)]
      [TestCase("int", "int?", false)]
      [TestCase("int?", "int?", false)]
      [TestCase("int?", "int", false)]
      [TestCase("int[]", "int[]", false)]
      [TestCase("int[]", "int?[]", false)]
      [TestCase("int?[]", "int?[]", false)]
      [TestCase("int?[]", "int[]", false)]
      [TestCase("string", "string", false)]
      [TestCase("DateTime", "DateTime", false)]
      [TestCase("DateTime[]", "DateTime[]", false)]
      [TestCase("DateTime?[]", "DateTime[]", false)]
      [TestCase("DateTime[]", "DateTime?[]", false)]
      [TestCase("DateTime?[]", "DateTime?[]", false)]
      [TestCase("Nested", "Nested", false)]
      [TestCase("Nested[]", "Nested[]", true)]
      [TestCase("ICollection<Nested>", "Nested[]", true)]
      [TestCase("IList<Nested>", "Nested[]", true)]
      [TestCase("IEnumerable<Nested>", "Nested[]", true)]
      [TestCase("IEnumerable<Nested>", "IEnumerable<Nested>", true)]
      [TestCase("ICollection<Nested>", "IEnumerable<Nested>", true)]
      [TestCase("IList<Nested>", "IEnumerable<Nested>", true)]
      [TestCase("Nested[]", "IEnumerable<Nested>", true)]
      [TestCase("IEnumerable<Nested>", "ICollection<Nested>", true)]
      [TestCase("ICollection<Nested>", "ICollection<Nested>", true)]
      [TestCase("IList<Nested>", "ICollection<Nested>", true)]
      [TestCase("Nested[]", "ICollection<Nested>", true)]
      [TestCase("IEnumerable<Nested>", "IList<Nested>", true)]
      [TestCase("ICollection<Nested>", "IList<Nested>", true)]
      [TestCase("IList<Nested>", "IList<Nested>", true)]
      [TestCase("Nested[]", "IList<Nested>", true)]
      [TestCase("IEnumerable<int>", "IEnumerable<int>", false)]
      [TestCase("IEnumerable<DateTime>", "IEnumerable<DateTime>", false)]
      public void ShouldReturn(string sourceType, string targetType, bool expectedResult) {
        var nestedSourceClass = TestUtilities.ParseClass($"public class Nested {{ public int Id {{ get; set; }} }}");
        var nestedTargetClass = TestUtilities.ParseClass($"public class Nested {{ public int Id {{ get; set; }} }}");
        var sourceClass =
          TestUtilities.ParseClass($"public class Source {{ public {sourceType} SourceProp {{ get; set; }} }}");
        var targetClass =
          TestUtilities.ParseClass($"public class Target {{ public {targetType} TargetProp {{ get; set; }} }}");
        var sourceProp = sourceClass.FindPropertyByName("SourceProp");
        var targetProp = targetClass.FindPropertyByName("TargetProp");
        var propertyMatch = new PropertyMatch {Source = sourceProp, Target = targetProp};
        A.CallTo(() => _classAdapterGenerationContext.Sources.Classes)
          .Returns(ImmutableList.Create(nestedSourceClass, sourceClass));
        A.CallTo(() => _classAdapterGenerationContext.Targets.Classes)
          .Returns(ImmutableList.Create(nestedTargetClass, targetClass));
        var nestedClassMatch = new ClassMatch {
          Source = nestedSourceClass,
          Target = nestedTargetClass,
          PropertyMatches = ImmutableList.Create<IPropertyMatch>(),
        };
        var nestedAdapter = new ClassAdapterBlueprint(nestedClassMatch, ImmutableList<IAdapterBlueprint>.Empty);
        A.CallTo(() => _blueprint.NestedAdapters).Returns(ImmutableList.Create<IAdapterBlueprint>(nestedAdapter));
        _sut.FindNestedAdapterForEnumerableAssigmnent(propertyMatch)
          .Should()
          .Be(expectedResult ? nestedAdapter : null,
            $"because mapping from {sourceType} to {targetType} should{(expectedResult ? "" : " not")} be possible using .Select( => ) and a nested adapter");
      }
    }
  }
}