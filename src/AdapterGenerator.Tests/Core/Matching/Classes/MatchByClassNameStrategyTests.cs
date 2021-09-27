using System.Linq;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Classes {
  [TestFixture]
  public class MatchByClassNameStrategyTests {
    MatchByClassNameStrategy _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new MatchByClassNameStrategy();
    }

    [TestFixture]
    public class Constructor : MatchByClassNameStrategyTests {
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
    public class Matches : MatchByClassNameStrategyTests {
      private IClass _source;
      private IClass _target1;
      private IClass _target2;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        var simpleInputClass = TestUtilities.ExtractClasses(TestDataIndex.Core.SimpleInputClass).Single();
        var simpleOutputClass = TestUtilities.ExtractClasses(TestDataIndex.Core.SimpleOutputClass).Single();
        var otherSimpleOutputClass = TestUtilities.ExtractClasses(TestDataIndex.Core.OtherSimpleOutputClass).Single();
        _source = simpleInputClass;
        _target1 = simpleOutputClass;
        _target2 = otherSimpleOutputClass;
      }

      [Test]
      public void ShouldMatchClassesByName() {
        _sut.Matches(_source, _target1).Should().BeTrue();
        _sut.Matches(_source, _target2).Should().BeFalse();
      }
    }
  }
}