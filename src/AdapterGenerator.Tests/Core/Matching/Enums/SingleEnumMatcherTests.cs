using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Enums {
  [TestFixture]
  public class SingleEnumMatcherTests {
    SingleEnumMatcher _sut;
    private IEnumerable<IEnumMatchingStrategy> _matchingStrategies;
    private IEnumMatchingStrategy _strategy1;
    private IEnumMatchingStrategy _strategy2;
    private IEnumMatchingStrategy _strategy3;
    private IEnumValueMatcher _enumValueMatcher;

    [SetUp]
    public virtual void SetUp() {
      _strategy1 = _strategy1.Fake();
      _strategy2 = _strategy2.Fake();
      _strategy3 = _strategy3.Fake();
      _matchingStrategies = new[] {_strategy1, _strategy2, _strategy3};
      _enumValueMatcher = _enumValueMatcher.Fake();
      _sut = new SingleEnumMatcher(new ConsoleLogger(), _matchingStrategies, _enumValueMatcher);
    }

    [TestFixture]
    public class Constructor : SingleEnumMatcherTests {
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
    public class Match : SingleEnumMatcherTests {
      private IEnum _source;
      private IImmutableList<IEnum> _targets;
      private IEnum _target1;
      private IEnum _target2;
      private IEnum _target3;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _target1 = TestUtilities.ParseEnum("public enum Target1{}");
        _target2 = TestUtilities.ParseEnum("public enum Target2{}");
        _target3 = TestUtilities.ParseEnum("public enum Target3{}");
        _source = TestUtilities.ParseEnum("public enum Source {}");
        _targets = ImmutableList.Create(_target1, _target2, _target3);
      }

      void StrategyMatchesTargets(IEnumMatchingStrategy strategy, IImmutableList<IEnum> targets) {
        A.CallTo(() => strategy.Matches(_source, _target1)).Returns(targets.Contains(_target1));
        A.CallTo(() => strategy.Matches(_source, _target2)).Returns(targets.Contains(_target2));
        A.CallTo(() => strategy.Matches(_source, _target3)).Returns(targets.Contains(_target3));
      }

      [Test]
      public void WhenTheFirstStrategyAlreadyNarrowsItDownToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1));
        var enumValueMatches = ImmutableList<IEnumValueMatch>.Empty;
        A.CallTo(() => _enumValueMatcher.Match(_source, _target1)).Returns(enumValueMatches);
        var match = _sut.Match(_source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target1);
        match.ValueMatches.Should().BeSameAs(enumValueMatches);
      }

      [Test]
      public void WhenAllStrategiesReturnNoTargetsThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList<IEnum>.Empty);
        StrategyMatchesTargets(_strategy2, ImmutableList<IEnum>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IEnum>.Empty);
        var match = _sut.Match(_source, _targets);
        match.Should().BeNull();
      }

      [Test]
      public void
        WhenTheFirstStrategyNarrowsItDownToTwoTargetsAndTheSecondStrategyToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList.Create(_target3));
        var enumValueMatches = ImmutableList<IEnumValueMatch>.Empty;
        A.CallTo(() => _enumValueMatcher.Match(_source, _target3)).Returns(enumValueMatches);
        var match = _sut.Match(_source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target3);
        match.ValueMatches.Should().BeSameAs(enumValueMatches);
      }

      [Test]
      public void WhenTheNoneOfTheStrategiesNarrowItDownToOneThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList<IEnum>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IEnum>.Empty);
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