using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class PropertyTestStatementsForDefaultValueGeneratorTests {
    PropertyTestStatementsForDefaultValueGenerator _sut;
    private IClassAdapterTestsGenerationContextWithClass _context;
    private IProperty _property;

    [SetUp]
    public virtual void SetUp() {
      _context = _context.Fake();
      _property = _property.Fake();
      _sut = new PropertyTestStatementsForDefaultValueGenerator(_context, _property);
    }

    [TestFixture]
    public class Constructor : PropertyTestStatementsForDefaultValueGeneratorTests {
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
    public class Generate : PropertyTestStatementsForDefaultValueGeneratorTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
        A.CallTo(() => _property.PropertyDeclarationSyntax)
          .Returns(PropertyDeclaration(NullableType(IdentifierName("DateTime")), "BirthDate"));
      }

      [Test]
      public void ShouldGenerateStatements() {
        _sut.Generate().Should().NotBeNull().And.NotBeEmpty();
      }

      [Test]
      public void ShouldGenerateCorrectStatements() {
        var statements = _sut.Generate();
        TestUtilities.FormatCode(statements[0]).ShouldBeSameCodeAs("var target = _sut.Adapt(_source);");
        TestUtilities.FormatCode(statements[1])
          .ShouldBeSameCodeAs("target.BirthDate.Should().Be(default(DateTime?));// TODO");
      }
    }
  }
}