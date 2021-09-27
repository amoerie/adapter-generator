using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters {
  [TestFixture]
  public class AdaptMethodBodyGeneratorTests {
    AdaptMethodBodyGenerator _sut;
    private IPropertySpecificStatementsGenerator _propertySpecificStatementsGenerator;

    [SetUp]
    public virtual void SetUp() {
      _propertySpecificStatementsGenerator = _propertySpecificStatementsGenerator.Fake();
      _sut = new AdaptMethodBodyGenerator(_propertySpecificStatementsGenerator);
    }

    [TestFixture]
    public class Constructor : AdaptMethodBodyGeneratorTests {
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
    public class GenerateAdaptMethodBodyForClassAdapter : AdaptMethodBodyGeneratorTests {
      private IClass _sourceClass;
      private IClass _targetClass;
      private IClassMatch _classMatch;
      private IClassAdapterGenerationContextWithClass _classAdapterGenerationContext;
      private IClassAdapterBlueprint _adapterBlueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _sourceClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Source).Single();
        _targetClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
        _classMatch = new ClassMatch {
          Source = _sourceClass,
          Target = _targetClass,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        _classAdapterGenerationContext = _classAdapterGenerationContext.Fake();
        _adapterBlueprint = _adapterBlueprint.Fake();
        A.CallTo(() => _classAdapterGenerationContext.Blueprint).Returns(_adapterBlueprint);
        A.CallTo(() => _adapterBlueprint.ClassMatch).Returns(_classMatch);
      }

      [Test]
      public void ShouldGenerateABody() {
        _sut.Generate(_classAdapterGenerationContext).Should().NotBeNull();
      }

     
    }

    [TestFixture]
    public class GenerateAdaptMethodBodyForEnumAdapter : AdaptMethodBodyGeneratorTests {
      private IEnum _source;
      private IEnum _target;
      private IEnumAdapterBlueprint _blueprint;
      private IEnumMatch _match;
      private IEnumAdapterGenerationContextWithClass _context;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = TestUtilities.ExtractEnums(TestDataIndex.Core.SourceEnum).Single();
        _target = TestUtilities.ExtractEnums(TestDataIndex.Core.TargetEnum).Single();
        _match = new EnumMatch {
          Source = _source,
          Target = _target,
          ValueMatches = ImmutableList<IEnumValueMatch>.Empty
        };
        _blueprint = _blueprint.Fake();
        _context = _context.Fake();
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.EnumMatch).Returns(_match);
      }

      [Test]
      public void ShouldGenerateMethodBody() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateBodyWithOnlyASwitchStatement() {
        var statements = _sut.Generate(_context).Statements;
        statements.Should().HaveCount(1);
        statements.OfType<SwitchStatementSyntax>().Should().NotBeEmpty().And.HaveCount(1);
      }

      [Test]
      public void ShouldGenerateCorrectBodyWhenThereAreNoMatches() {
        // unfortunately, due to a problem in the Roslyn code formatter, we can't write tests with matched values because the formatter crashes
        // when you provide a syntax node with labels.. :(
        var code = TestUtilities.FormatCode(_sut.Generate(_context));
        code.ShouldBeSameCodeAs(@"{
    switch (source)
    {
        default:
            throw new ArgumentOutOfRangeException(nameof(source), source, $""Cannot adapt {source} to a matching value"");
    }
}");
      }
    }
  }
}