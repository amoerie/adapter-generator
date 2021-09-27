using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.Properties {
  [TestFixture]
  public class PropertySpecificStatementsGeneratorTests {
    PropertySpecificStatementsGenerator _sut;
    private IPropertyAdaptingStatementsGeneratorDecider _decider;

    [SetUp]
    public virtual void SetUp() {
      _decider = _decider.Fake();
      _sut = new PropertySpecificStatementsGenerator(_decider);
    }

    [TestFixture]
    public class Constructor : PropertySpecificStatementsGeneratorTests {
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
    public class GeneratePropertySpecificStatements : PropertySpecificStatementsGeneratorTests {
      private IClassAdapterGenerationContextWithClass _context;
      private IProperty _property;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _property = _property.Fake();
      }

      [Test]
      public void ShouldReturnStatementsFromGenerator() {
        IPropertyAdaptingStatementsGenerator generator = A.Fake<IPropertyAdaptingStatementsGenerator>();
        A.CallTo(() => _decider.Decide(_context, _property)).Returns(generator);
        IImmutableList<StatementSyntax> statements = A.Fake<IImmutableList<StatementSyntax>>();
        A.CallTo(() => generator.Generate()).Returns(statements);
        _sut.Generate(_context, _property).Should().BeSameAs(statements);
      }
    }
  }
}