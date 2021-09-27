using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Classes.Properties {
  [TestFixture]
  public class PropertyMatcherTests {
    PropertyMatcher _sut;
    private ISinglePropertyMatcher _singlePropertyMatcher;

    [SetUp]
    public virtual void SetUp() {
      _singlePropertyMatcher = _singlePropertyMatcher.Fake();
      _sut = new PropertyMatcher(_singlePropertyMatcher);
    }

    [TestFixture]
    public class Constructor : PropertyMatcherTests {
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
    public class Match : PropertyMatcherTests {
      private IClass _source;
      private IClass _target;
      private ImmutableList<IProperty> _sourceProperties;
      private ImmutableList<IProperty> _targetProperties;
      private IImmutableList<IClassMatchWithoutPropertyMatches> _classMatches;
      private IImmutableList<IEnumMatch> _enumMatches;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = _source.Fake();
        _sourceProperties = ImmutableList.Create(A.Fake<IProperty>(), A.Fake<IProperty>());
        _target = _source.Fake();
        _targetProperties = ImmutableList.Create(A.Fake<IProperty>(), A.Fake<IProperty>());
        _classMatches = _classMatches.Fake();
        _enumMatches = _enumMatches.Fake();
        A.CallTo(() => _source.Properties).Returns(_sourceProperties);
        A.CallTo(() => _target.Properties).Returns(_targetProperties);
      }

      [Test]
      public void ShouldReturnMatchedSourcesAndFilterOutNulls() {
        var match = new PropertyMatch();
        A.CallTo(
          () => _singlePropertyMatcher.Match(_classMatches, _enumMatches, _source.Properties[0], _target.Properties))
          .Returns(match);
        A.CallTo(
          () => _singlePropertyMatcher.Match(_classMatches, _enumMatches, _source.Properties[1], _target.Properties))
          .Returns(null);
        _sut.Match(_classMatches, _enumMatches, _source, _target)
          .Should()
          .NotBeNull()
          .And.NotBeEmpty()
          .And.HaveCount(1)
          .And.Contain(match);
      }
    }
  }
}