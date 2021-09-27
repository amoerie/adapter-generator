using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsPropertyTestMethodGeneratorTests {
    AdapterTestsPropertyTestMethodGenerator _sut;
    private IAdapterTestsPropertySpecificTestStatementsGenerator _propertySpecificStatementsGenerator;

    [SetUp]
    public virtual void SetUp() {
      _propertySpecificStatementsGenerator = _propertySpecificStatementsGenerator.Fake();
      _sut = new AdapterTestsPropertyTestMethodGenerator(_propertySpecificStatementsGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterTestsPropertyTestMethodGeneratorTests {
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
    public class Generate : AdapterTestsPropertyTestMethodGeneratorTests {
      private IClassAdapterTestsGenerationContextWithClass _context;
      private IProperty _property;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _property = _property.Fake();
        A.CallTo(() => _property.PropertyDeclarationSyntax)
          .Returns(PropertyDeclaration(IdentifierName("DateTime"), "BirthDate"));
        StatementSyntax trueShouldBeTrue = ExpressionStatement(
          InvocationExpression(
            MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              InvocationExpression(
                MemberAccessExpression(
                  SyntaxKind.SimpleMemberAccessExpression,
                  LiteralExpression(SyntaxKind.TrueLiteralExpression),
                  IdentifierName("Should"))),
              IdentifierName("BeTrue"))));
        A.CallTo(() => _propertySpecificStatementsGenerator.Generate(_context, _property))
          .Returns(ImmutableList.Create(trueShouldBeTrue));
      }

      [Test]
      public void ShouldGenerateTestMethod() {
        _sut.Generate(_context, _property).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateCorrectTestMethod() {
        TestUtilities.FormatCode(_sut.Generate(_context, _property)).ShouldBeSameCodeAs(@"[Test]
public void ShouldAdaptBirthDate()
{
    true.Should().BeTrue();
}");
      }
    }
  }
}