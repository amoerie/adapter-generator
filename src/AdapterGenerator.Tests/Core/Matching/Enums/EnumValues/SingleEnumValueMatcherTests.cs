using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Enums.EnumValues {
  [TestFixture]
  public class SingleEnumValueMatcherTests {
    [SetUp]
    public virtual void SetUp() {
      _strategy1 = _strategy1.Fake();
      _strategy2 = _strategy2.Fake();
      _strategy3 = _strategy3.Fake();
      _matchingStrategies = new[] {_strategy1, _strategy2, _strategy3};
      _sut = new SingleEnumValueMatcher(new ConsoleLogger(), _matchingStrategies);
    }

    private SingleEnumValueMatcher _sut;
    private IEnumerable<IEnumValueMatchingStrategy> _matchingStrategies;
    private IEnumValueMatchingStrategy _strategy1;
    private IEnumValueMatchingStrategy _strategy2;
    private IEnumValueMatchingStrategy _strategy3;

    [TestFixture]
    public class Constructor : SingleEnumValueMatcherTests {
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
    public class Match : SingleEnumValueMatcherTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _target1 = TestUtilities.ParseEnum("public enum Target1{ Id }").FindEnumValueByName("Id");
        _target2 = TestUtilities.ParseEnum("public enum Target2{ Id }").FindEnumValueByName("Id");
        _target3 = TestUtilities.ParseEnum("public enum Target3{ Id }").FindEnumValueByName("Id");
        _source = TestUtilities.ParseEnum("public enum Source { Id}").FindEnumValueByName("Id");
        _targets = ImmutableList.Create(_target1, _target2, _target3);
      }

      private IEnumValue _source;
      private IImmutableList<IEnumValue> _targets;
      private IEnumValue _target1;
      private IEnumValue _target2;
      private IEnumValue _target3;

      private void StrategyMatchesTargets(IEnumValueMatchingStrategy strategy, IImmutableList<IEnumValue> targets) {
        A.CallTo(() => strategy.Matches(_source, _target1)).Returns(targets.Contains(_target1));
        A.CallTo(() => strategy.Matches(_source, _target2)).Returns(targets.Contains(_target2));
        A.CallTo(() => strategy.Matches(_source, _target3)).Returns(targets.Contains(_target3));
      }

      [Test]
      public void WhenAllStrategiesReturnNoTargetsThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList<IEnumValue>.Empty);
        StrategyMatchesTargets(_strategy2, ImmutableList<IEnumValue>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IEnumValue>.Empty);
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

      [Test]
      public void WhenTheFirstStrategyAlreadyNarrowsItDownToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1));
        var match = _sut.Match(_source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target1);
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
        StrategyMatchesTargets(_strategy2, ImmutableList<IEnumValue>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IEnumValue>.Empty);
        var match = _sut.Match(_source, _targets);
        match.Should().BeNull();
      }
    }
  }
}