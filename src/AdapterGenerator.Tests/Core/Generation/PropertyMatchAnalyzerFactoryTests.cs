using AdapterGenerator.Core.Generation;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation {
  [TestFixture]
  public class PropertyMatchAnalyzerFactoryTests {
    PropertyMatchAnalyzerFactory _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new PropertyMatchAnalyzerFactory();
    }

    [TestFixture]
    public class Constructor : PropertyMatchAnalyzerFactoryTests {
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
    public class CreateAnalyzer : PropertyMatchAnalyzerFactoryTests {
      private IClassAdapterGenerationContextWithClass _classAdapterGenerationContext;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _classAdapterGenerationContext = _classAdapterGenerationContext.Fake();
      }

      [Test]
      public void ShouldCreatePropertyMatchAnalyzer() {
        _sut.Create(_classAdapterGenerationContext).Should().NotBeNull().And.BeOfType<PropertyMatchAnalyzer>();
      }
    }
  }
}