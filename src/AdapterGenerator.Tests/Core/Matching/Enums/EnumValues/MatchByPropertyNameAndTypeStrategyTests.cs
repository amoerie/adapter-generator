using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Enums.EnumValues {
  [TestFixture]
  public class MatchByEnumValueNameAndTypeStrategyTests {
    MatchByEnumValueNameStrategy _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new MatchByEnumValueNameStrategy();
    }

    [TestFixture]
    public class Constructor : MatchByEnumValueNameAndTypeStrategyTests {
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
    public class Matches : MatchByEnumValueNameAndTypeStrategyTests {
      private IEnumValue _maleSource;
      private IEnumValue _maleTarget;
      private IEnumValue _mexicanTarget;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _maleSource = TestUtilities.ParseEnum("public enum Gender { Male, Female }").FindEnumValueByName("Male");
        _maleTarget = TestUtilities.ParseEnum("public enum Gender { Male, Female }").FindEnumValueByName("Male");
        _mexicanTarget = TestUtilities.ParseEnum("public enum Gender { Mexican }").FindEnumValueByName("Mexican");
      }

      [Test]
      public void WhenTheNamesAreEqualShouldBeAMatch() {
        _sut.Matches(_maleSource, _maleTarget).Should().BeTrue();
      }

      [Test]
      public void WhenTheNamesAreNotEqualShouldNotBeAMatch() {
        _sut.Matches(_maleSource, _mexicanTarget).Should().BeFalse();
      }
    }
  }
}