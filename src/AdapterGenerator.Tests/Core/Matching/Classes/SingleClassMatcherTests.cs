using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Classes {
  [TestFixture]
  public class SingleClassMatcherTests {
    SingleClassMatcher _sut;
    private IEnumerable<IClassMatchingStrategy> _matchingStrategies;
    private IClassMatchingStrategy _strategy1;
    private IClassMatchingStrategy _strategy2;
    private IClassMatchingStrategy _strategy3;
    private IPropertyMatcher _propertyMatcher;

    [SetUp]
    public virtual void SetUp() {
      _strategy1 = _strategy1.Fake();
      _strategy2 = _strategy2.Fake();
      _strategy3 = _strategy3.Fake();
      _matchingStrategies = new[] {_strategy1, _strategy2, _strategy3};
      _propertyMatcher = _propertyMatcher.Fake();
      _sut = new SingleClassMatcher(new ConsoleLogger(), _matchingStrategies);
    }

    [TestFixture]
    public class Constructor : SingleClassMatcherTests {
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
    public class Match : SingleClassMatcherTests {
      private IClass _source;
      private IImmutableList<IClass> _targets;
      private IClass _target1;
      private IClass _target2;
      private IClass _target3;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _target1 = TestUtilities.ParseClass("public class Target1{}");
        _target2 = TestUtilities.ParseClass("public class Target2{}");
        _target3 = TestUtilities.ParseClass("public class Target3{}");
        _source = TestUtilities.ParseClass("public class Source {}");
        _targets = ImmutableList.Create(_target1, _target2, _target3);
      }

      void StrategyMatchesTargets(IClassMatchingStrategy strategy, IImmutableList<IClass> targets) {
        A.CallTo(() => strategy.Matches(_source, _target1)).Returns(targets.Contains(_target1));
        A.CallTo(() => strategy.Matches(_source, _target2)).Returns(targets.Contains(_target2));
        A.CallTo(() => strategy.Matches(_source, _target3)).Returns(targets.Contains(_target3));
      }

      [Test]
      public void WhenTheFirstStrategyAlreadyNarrowsItDownToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1));
        var propertyMatches = ImmutableList<IPropertyMatch>.Empty;
        var match = _sut.Match(_source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target1);
      }

      [Test]
      public void WhenAllStrategiesReturnNoTargetsThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList<IClass>.Empty);
        StrategyMatchesTargets(_strategy2, ImmutableList<IClass>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IClass>.Empty);
        var match = _sut.Match(_source, _targets);
        match.Should().BeNull();
      }

      [Test]
      public void
        WhenTheFirstStrategyNarrowsItDownToTwoTargetsAndTheSecondStrategyToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList.Create(_target3));
        var match = _sut.Match(_source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target3);
      }

      [Test]
      public void WhenTheNoneOfTheStrategiesNarrowItDownToOneThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList<IClass>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IClass>.Empty);
        var match = _sut.Match(_source, _targets);
        match.Should().BeNull();
      }

      [Test]
      public void WhenAllStrategiesReturnTwoTargetsRemainThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target2, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy3, ImmutableList.Create(_target1, _target2));
        var match = _sut.Match(_source, _targets);
        match.Should().BeNull();
      }
    }
  }
}