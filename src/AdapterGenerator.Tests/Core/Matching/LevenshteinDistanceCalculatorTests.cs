using AdapterGenerator.Core.Matching;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching {
  [TestFixture]
  public class LevenshteinDistanceCalculatorTests {
    LevenshteinDistanceCalculator _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new LevenshteinDistanceCalculator();
    }

    [TestFixture]
    public class Constructor : LevenshteinDistanceCalculatorTests {
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
    public class Calculate : LevenshteinDistanceCalculatorTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void WhenWordsAre1LetterDifferentShouldReturn1() {
        _sut.Calculate("kitten", "mitten").Should().Be(1);
        _sut.Calculate("sitten", "sittin").Should().Be(1);
        _sut.Calculate("sittin", "sitting").Should().Be(1);
        _sut.Calculate("not", "note").Should().Be(1);
        _sut.Calculate("note", "not").Should().Be(1);
      }

      [Test]
      public void WhenWordsAreTheSameShouldReturn0() {
        _sut.Calculate("kitten", "kitten").Should().Be(0);
        _sut.Calculate("note", "note").Should().Be(0);
      }

      [Test]
      public void WhenOnly2ChangesAreNecessaryToTransformTheFirstStringIntoTheSecondShouldReturn2() {
        _sut.Calculate("flaw", "lawn").Should().Be(2);
      }
    }
  }
}