using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Classes.Properties {
  [TestFixture]
  public class SinglePropertyMatcherTests {
    [SetUp]
    public virtual void SetUp() {
      _strategy1 = _strategy1.Fake();
      _strategy2 = _strategy2.Fake();
      _strategy3 = _strategy3.Fake();
      _matchingStrategies = new[] {_strategy1, _strategy2, _strategy3};
      _factory = _factory.Fake();
      _classMatchWithoutPropertyMatches = _classMatchWithoutPropertyMatches.Fake();
      _enumMatches = _enumMatches.Fake();
      A.CallTo(() => _factory.Create(_classMatchWithoutPropertyMatches, _enumMatches))
        .Returns(_matchingStrategies.ToImmutableList());
      _sut = new SinglePropertyMatcher(new ConsoleLogger(), _factory);
    }

    private SinglePropertyMatcher _sut;
    private IEnumerable<IPropertyMatchingStrategy> _matchingStrategies;
    private IPropertyMatchingStrategy _strategy1;
    private IPropertyMatchingStrategy _strategy2;
    private IPropertyMatchingStrategy _strategy3;
    private IPropertyMatchingStrategiesFactory _factory;
    private IImmutableList<IClassMatchWithoutPropertyMatches> _classMatchWithoutPropertyMatches;
    private IImmutableList<IEnumMatch> _enumMatches;

    [TestFixture]
    public class Constructor : SinglePropertyMatcherTests {
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
    public class Match : SinglePropertyMatcherTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _target1 =
          TestUtilities.ParseClass("public class Target1{ public int Id { get; set; } }").FindPropertyByName("Id");
        _target2 =
          TestUtilities.ParseClass("public class Target2{ public int Id { get; set; } }").FindPropertyByName("Id");
        _target3 =
          TestUtilities.ParseClass("public class Target3{ public int Id { get; set; } }").FindPropertyByName("Id");
        _source =
          TestUtilities.ParseClass("public class Source { public int Id { get; set; } }").FindPropertyByName("Id");
        _targets = ImmutableList.Create(_target1, _target2, _target3);
      }

      private IProperty _source;
      private IImmutableList<IProperty> _targets;
      private IProperty _target1;
      private IProperty _target2;
      private IProperty _target3;

      private void StrategyMatchesTargets(IPropertyMatchingStrategy strategy, IImmutableList<IProperty> targets) {
        A.CallTo(() => strategy.Matches(_source, _target1)).Returns(targets.Contains(_target1));
        A.CallTo(() => strategy.Matches(_source, _target2)).Returns(targets.Contains(_target2));
        A.CallTo(() => strategy.Matches(_source, _target3)).Returns(targets.Contains(_target3));
      }

      [Test]
      public void WhenAllStrategiesReturnNoTargetsThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList<IProperty>.Empty);
        StrategyMatchesTargets(_strategy2, ImmutableList<IProperty>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IProperty>.Empty);
        var match = _sut.Match(_classMatchWithoutPropertyMatches, _enumMatches, _source, _targets);
        match.Should().BeNull();
      }

      [Test]
      public void WhenAllStrategiesReturnTwoTargetsRemainThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target2, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy3, ImmutableList.Create(_target1, _target2));
        var match = _sut.Match(_classMatchWithoutPropertyMatches, _enumMatches, _source, _targets);
        match.Should().BeNull();
      }

      [Test]
      public void WhenTheFirstStrategyAlreadyNarrowsItDownToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1));
        var match = _sut.Match(_classMatchWithoutPropertyMatches, _enumMatches, _source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target1);
      }

      [Test]
      public void
        WhenTheFirstStrategyNarrowsItDownToTwoTargetsAndTheSecondStrategyToOneTargetThenItShouldReturnThatMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList.Create(_target3));
        var match = _sut.Match(_classMatchWithoutPropertyMatches, _enumMatches, _source, _targets);
        match.Should().NotBeNull();
        match.Source.Should().Be(_source);
        match.Target.Should().Be(_target3);
      }

      [Test]
      public void WhenTheNoneOfTheStrategiesNarrowItDownToOneThenItShouldReturnNoMatch() {
        StrategyMatchesTargets(_strategy1, ImmutableList.Create(_target1, _target3));
        StrategyMatchesTargets(_strategy2, ImmutableList<IProperty>.Empty);
        StrategyMatchesTargets(_strategy3, ImmutableList<IProperty>.Empty);
        var match = _sut.Match(_classMatchWithoutPropertyMatches, _enumMatches, _source, _targets);
        match.Should().BeNull();
      }
    }
  }
}