using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsConstructorTestsGeneratorTests {
    AdapterTestsConstructorTestsGenerator _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new AdapterTestsConstructorTestsGenerator();
    }

    [TestFixture]
    public class Constructor : AdapterTestsConstructorTestsGeneratorTests {
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
    public class Generate : AdapterTestsConstructorTestsGeneratorTests {
      private IClassAdapterTestsGenerationContextWithClass _context;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        A.CallTo(() => _context.TestClassName).Returns("PokémonAdapter");
      }

      [Test]
      public void ShouldGenerateConstructorTests() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateCorrectConstructorTests() {
        TestUtilities.FormatCode(_sut.Generate(_context)).ShouldBeSameCodeAs(@"[TestFixture]
public class Constructor : PokémonAdapter
{
    [Test]
    public void ShouldHaveNoOptionalDependencies()
    {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
    }
}");
      }
    }
  }
}