using System.Collections.Immutable;
using AdapterGenerator.Core.Generation;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.Properties {
  [TestFixture]
  public class PropertyAdaptingStatementsGeneratorDeciderTests {
    PropertyAdaptingStatementsGeneratorDecider _sut;
    IPropertyMatchAnalyzerFactory _propertyMatchAnalyzerFactory;
    ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;

    [SetUp]
    public virtual void SetUp() {
      _typeSyntaxAnalyzer = _typeSyntaxAnalyzer.Fake();
      _propertyMatchAnalyzerFactory = _propertyMatchAnalyzerFactory.Fake();
      _sut = new PropertyAdaptingStatementsGeneratorDecider(_propertyMatchAnalyzerFactory, _typeSyntaxAnalyzer);
    }

    [TestFixture]
    public class Constructor : PropertyAdaptingStatementsGeneratorDeciderTests {
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
    public class Decide : PropertyAdaptingStatementsGeneratorDeciderTests {
      private IClassAdapterGenerationContextWithClass _context;
      private IProperty _property;
      private IClassAdapterBlueprint _blueprint;
      private IClassMatch _classMatch;
      private IClass _source;
      private IClass _target;
      private PropertyMatch _idMatch;
      private IProperty _targetIdProperty;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _property = _property.Fake();
        _blueprint = _blueprint.Fake();
        _source = TestUtilities.ParseClass("public class Source { public int Id { get; set; } }");
        _target = TestUtilities.ParseClass("public class Target { public int Id { get; set; } }");
        _targetIdProperty = _target.FindPropertyByName("Id");
        _idMatch = new PropertyMatch {
          Source = _source.FindPropertyByName("Id"),
          Target = _targetIdProperty
        };
        _classMatch = new ClassMatch {
          Source = _source,
          Target = _target,
          PropertyMatches = ImmutableList.Create<IPropertyMatch>(_idMatch)
        };
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.ClassMatch).Returns(_classMatch);
      }

      [Test]
      public void WhenThereAreNoPropertyMatchesForTheProvidedPropertyShouldReturnAssignDefaultValueStatementsGenerator() {
        _sut.Decide(_context, A.Fake<IProperty>()).Should().BeOfType<AssignDefaultValueStatementsGenerator>();
      }

      [Test]
      public void WhenPropertyCanBeAdaptedWithSimpleAssignmentShouldReturnSimpleAssignmentStatementsGenerator() {
        IPropertyMatchAnalyzer analyzer = A.Fake<IPropertyMatchAnalyzer>();
        A.CallTo(() => _propertyMatchAnalyzerFactory.Create(_context)).Returns(analyzer);
        A.CallTo(() => analyzer.CanBeAdaptedThroughSimpleAssignment(_idMatch)).Returns(true);
        _sut.Decide(_context, _targetIdProperty).Should().BeOfType<SimpleAssignmentStatementsGenerator>();
      }

      [Test]
      public void WhenPropertyCanBeAdaptedWithNestedAdapterShouldReturnNestedAdapterStatementsGenerator() {
        IPropertyMatchAnalyzer analyzer = A.Fake<IPropertyMatchAnalyzer>();
        A.CallTo(() => _propertyMatchAnalyzerFactory.Create(_context)).Returns(analyzer);
        A.CallTo(() => analyzer.FindNestedAdapterForEnumerableAssigmnent(_idMatch)).Returns(null);
        A.CallTo(() => analyzer.FindNestedAdapterForSimpleAssignment(_idMatch)).Returns(A.Fake<IAdapterBlueprint>());
        _sut.Decide(_context, _targetIdProperty).Should().BeOfType<NestedAdapterStatementsGenerator>();
      }

      [Test]
      public void WhenEnumerablePropertyCanBeAdaptedWithNestedAdapterShouldReturnNestedAdapterStatementsGenerator() {
        IPropertyMatchAnalyzer analyzer = A.Fake<IPropertyMatchAnalyzer>();
        A.CallTo(() => _propertyMatchAnalyzerFactory.Create(_context)).Returns(analyzer);
        A.CallTo(() => analyzer.FindNestedAdapterForSimpleAssignment(_idMatch)).Returns(null);
        A.CallTo(() => analyzer.FindNestedAdapterForEnumerableAssigmnent(_idMatch))
          .Returns(A.Fake<IAdapterBlueprint>());
        _sut.Decide(_context, _targetIdProperty).Should().BeOfType<NestedAdapterForIEnumerableStatementsGenerator>();
      }

      [Test]
      public void WhenPropertyCannotBeAdaptedShouldReturnAssignDefaultValueStatementsGenerator() {
        IPropertyMatchAnalyzer analyzer = A.Fake<IPropertyMatchAnalyzer>();
        A.CallTo(() => _propertyMatchAnalyzerFactory.Create(_context)).Returns(analyzer);
        A.CallTo(() => analyzer.FindNestedAdapterForSimpleAssignment(_idMatch)).Returns(null);
        A.CallTo(() => analyzer.FindNestedAdapterForEnumerableAssigmnent(_idMatch)).Returns(null);
        _sut.Decide(_context, _targetIdProperty).Should().BeOfType<AssignDefaultValueStatementsGenerator>();
      }
    }
  }
}