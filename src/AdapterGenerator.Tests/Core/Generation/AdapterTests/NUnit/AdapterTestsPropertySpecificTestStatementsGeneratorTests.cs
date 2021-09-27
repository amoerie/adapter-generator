using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsPropertySpecificTestStatementsGeneratorTests {
    AdapterTestsPropertySpecificTestStatementsGenerator _sut;
    private IPropertyTestStatementsGeneratorFactory _factory;

    [SetUp]
    public virtual void SetUp() {
      _factory = _factory.Fake();
      _sut = new AdapterTestsPropertySpecificTestStatementsGenerator(_factory);
    }

    [TestFixture]
    public class Constructor : AdapterTestsPropertySpecificTestStatementsGeneratorTests {
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
    public class Generate : AdapterTestsPropertySpecificTestStatementsGeneratorTests {
      private IClassAdapterTestsGenerationContextWithClass _context;
      private IProperty _property;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _property = _property.Fake();
        _sut.Generate(_context, _property);
      }

      [Test]
      public void ShouldReturnTheGeneratedStatementsFromTheGenerator() {
        IPropertyTestStatementsGenerator generator = A.Fake<IPropertyTestStatementsGenerator>();
        A.CallTo(() => _factory.Create(_context, _property)).Returns(generator);
        IImmutableList<StatementSyntax> statements = A.Fake<IImmutableList<StatementSyntax>>();
        A.CallTo(() => generator.Generate()).Returns(statements);
        _sut.Generate(_context, _property).Should().BeSameAs(statements);
      }
    }
  }
}