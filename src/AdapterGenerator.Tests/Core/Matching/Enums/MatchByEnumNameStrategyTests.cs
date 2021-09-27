using System.Linq;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Enums {
  [TestFixture]
  public class MatchByEnumNameStrategyTests {
    MatchByEnumNameStrategy _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new MatchByEnumNameStrategy();
    }

    [TestFixture]
    public class Constructor : MatchByEnumNameStrategyTests {
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
    public class Matches : MatchByEnumNameStrategyTests {
      private IEnum _source;
      private IEnum _target1;
      private IEnum _target2;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        var simpleInputEnum = TestUtilities.ExtractEnums(TestDataIndex.Core.SimpleInputEnum).Single();
        var simpleOutputEnum = TestUtilities.ExtractEnums(TestDataIndex.Core.SimpleOutputEnum).Single();
        var otherSimpleOutputEnum = TestUtilities.ExtractEnums(TestDataIndex.Core.OtherSimpleOutputEnum).Single();
        _source = simpleInputEnum;
        _target1 = simpleOutputEnum;
        _target2 = otherSimpleOutputEnum;
      }

      [Test]
      public void ShouldMatchEnumsByName() {
        _sut.Matches(_source, _target1).Should().BeTrue();
        _sut.Matches(_source, _target2).Should().BeFalse();
      }
    }
  }
}