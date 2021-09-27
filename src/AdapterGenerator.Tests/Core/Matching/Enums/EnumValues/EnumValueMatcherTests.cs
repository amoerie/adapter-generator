using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Enums.EnumValues {
  [TestFixture]
  public class EnumValueMatcherTests {
    EnumValueMatcher _sut;
    private ISingleEnumValueMatcher _singleEnumValueMatcher;

    [SetUp]
    public virtual void SetUp() {
      _singleEnumValueMatcher = _singleEnumValueMatcher.Fake();
      _sut = new EnumValueMatcher(_singleEnumValueMatcher);
    }

    [TestFixture]
    public class Constructor : EnumValueMatcherTests {
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
    public class Match : EnumValueMatcherTests {
      private IEnum _source;
      private IEnum _target;
      private ImmutableList<IEnumValue> _sourceEnumValues;
      private ImmutableList<IEnumValue> _targetEnumValues;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = _source.Fake();
        _sourceEnumValues = ImmutableList.Create(A.Fake<IEnumValue>(), A.Fake<IEnumValue>());
        _target = _source.Fake();
        _targetEnumValues = ImmutableList.Create(A.Fake<IEnumValue>(), A.Fake<IEnumValue>());
        A.CallTo(() => _source.Values).Returns(_sourceEnumValues);
        A.CallTo(() => _target.Values).Returns(_targetEnumValues);
      }

      [Test]
      public void ShouldReturnMatchedSourcesAndFilterOutNulls() {
        var match = new EnumValueMatch();
        A.CallTo(() => _singleEnumValueMatcher.Match(_source.Values[0], _target.Values))
          .Returns(match);
        A.CallTo(() => _singleEnumValueMatcher.Match(_source.Values[1], _target.Values))
          .Returns(null);
        _sut.Match(_source, _target)
          .Should()
          .NotBeNull()
          .And.NotBeEmpty()
          .And.HaveCount(1)
          .And.Contain(match);
      }
    }
  }
}