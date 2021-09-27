using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsAdaptMethodTestsGeneratorTests {
    AdapterTestsAdaptMethodTestsGenerator _sut;
    private IAdapterTestsPropertyTestMethodGenerator _propertyTestMethodGenerator;

    [SetUp]
    public virtual void SetUp() {
      _propertyTestMethodGenerator = _propertyTestMethodGenerator.Fake();
      _sut = new AdapterTestsAdaptMethodTestsGenerator(_propertyTestMethodGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterTestsAdaptMethodTestsGeneratorTests {
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
    public class Generate : AdapterTestsAdaptMethodTestsGeneratorTests {
      private IClassAdapterTestsGenerationContextWithClass _context;
      private IGeneratedClassAdapter _adapter;
      private IClassAdapterBlueprint _blueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _adapter = _adapter.Fake();
        _blueprint = _blueprint.Fake();
        var simpleAdapter = TestUtilities.ExtractClasses(TestDataIndex.Core.SimpleAdapter).Single();
        A.CallTo(() => _context.Adapter).Returns(_adapter);
        A.CallTo(() => _context.TestClassName).Returns("TargetAdapterTests");
        A.CallTo(() => _adapter.Blueprint).Returns(_blueprint);
        A.CallTo(() => _adapter.CompilationUnitSyntax).Returns(simpleAdapter.CompilationUnitSyntax);
        A.CallTo(() => _blueprint.Name).Returns(IdentifierName("SimpleAdapter"));
        A.CallTo(() => _blueprint.QualifiedName).Returns(IdentifierName("SimpleAdapter"));
        A.CallTo(() => _blueprint.ClassMatch.Source.QualifiedName).Returns(ParseName("FullyQualified.Source"));
        A.CallTo(() => _blueprint.ClassMatch.Target.Properties).Returns(ImmutableList.Create(A.Fake<IProperty>()));
        A.CallTo(() => _propertyTestMethodGenerator.Generate(_context, A<IProperty>._))
          .Returns(
            MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("SomePropertyTest"))
              .WithBody(Block()));
      }

      [Test]
      public void ShouldGenerateAdaptMethodTests() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateCorrectTests() {
        var code = TestUtilities.FormatCode(_sut.Generate(_context));
        code.ShouldBeSameCodeAs(@"[TestFixture]
public class Adapt : TargetAdapterTests
{
    private FullyQualified.Source _source;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        _source = new FullyQualified.Source();
    }

    [Test]
    public void ShouldReturnNullWhenSourceIsNull()
    {
        _source = null;
        var target = _sut.Adapt(_source);
        target.Should().BeNull();
    }

    void SomePropertyTest()
    {
    }
}");
      }
    }
  }
}