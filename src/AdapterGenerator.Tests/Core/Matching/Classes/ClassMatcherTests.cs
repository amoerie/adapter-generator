using System.Collections.Immutable;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Classes {
  [TestFixture]
  public class ClassMatcherTests {
    ClassMatcher _sut;
    private ISingleClassMatcher _singleClassMatcher;

    [SetUp]
    public virtual void SetUp() {
      _singleClassMatcher = _singleClassMatcher.Fake();
      _sut = new ClassMatcher(_singleClassMatcher);
    }

    [TestFixture]
    public class Constructor : ClassMatcherTests {
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
    public class Match : ClassMatcherTests {
      private IImmutableList<IClass> _sources;
      private IImmutableList<IClass> _targets;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _sources = ImmutableList.Create(A.Fake<IClass>(), A.Fake<IClass>());
        _targets = ImmutableList.Create(A.Fake<IClass>(), A.Fake<IClass>());
      }

      [Test]
      public void ShouldReturnMatchedSourcesAndFilterOutNulls() {
        var match = new ClassMatchWithoutPropertyMatches();
        A.CallTo(() => _singleClassMatcher.Match(_sources[0], _targets))
          .Returns(match);
        A.CallTo(() => _singleClassMatcher.Match(_sources[1], _targets))
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