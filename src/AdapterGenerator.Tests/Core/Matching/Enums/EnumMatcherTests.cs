using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Enums {
  [TestFixture]
  public class EnumMatcherTests {
    EnumMatcher _sut;
    private ISingleEnumMatcher _singleEnumMatcher;

    [SetUp]
    public virtual void SetUp() {
      _singleEnumMatcher = _singleEnumMatcher.Fake();
      _sut = new EnumMatcher(_singleEnumMatcher);
    }

    [TestFixture]
    public class Constructor : EnumMatcherTests {
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
    public class Match : EnumMatcherTests {
      private IImmutableList<IEnum> _sources;
      private IImmutableList<IEnum> _targets;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _sources = ImmutableList.Create(A.Fake<IEnum>(), A.Fake<IEnum>());
        _targets = ImmutableList.Create(A.Fake<IEnum>(), A.Fake<IEnum>());
      }

      [Test]
      public void ShouldReturnMatchedSourcesAndFilterOutNulls() {
        var match = new EnumMatch();
        A.CallTo(() => _singleEnumMatcher.Match(_sources[0], _targets))
          .Returns(match);
        A.CallTo(() => _singleEnumMatcher.Match(_sources[1], _targets))
          .Returns(null);
        _sut.Match(_sources, _targets)
          .Should()
          .NotBeNull()
          .And.NotBeEmpty()
          .And.HaveCount(1)
          .And.Contain(match);
      }
    }
  }
}