using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsSetupMethodGeneratorTests {
    AdapterTestsSetupMethodGenerator _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new AdapterTestsSetupMethodGenerator();
    }

    [TestFixture]
    public class Constructor : AdapterTestsSetupMethodGeneratorTests {
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
    public class Generate : AdapterTestsSetupMethodGeneratorTests {
      private IClassAdapterTestsGenerationContextWithClass _context;
      private IGeneratedClassAdapter _adapter;
      private IClassAdapterBlueprint _blueprint;
      private IAdapterBlueprint _nestedAdapter;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _adapter = _adapter.Fake();
        _blueprint = _blueprint.Fake();
        _nestedAdapter = _nestedAdapter.Fake();
        var simpleAdapter = TestUtilities.ExtractClasses(TestDataIndex.Core.SimpleAdapter).Single();
        A.CallTo(() => _context.Adapter).Returns(_adapter);
        A.CallTo(() => _adapter.Blueprint).Returns(_blueprint);
        A.CallTo(() => _adapter.CompilationUnitSyntax).Returns(simpleAdapter.CompilationUnitSyntax);
        A.CallTo(() => _blueprint.Name).Returns(IdentifierName("SimpleAdapter"));
        A.CallTo(() => _blueprint.QualifiedName).Returns(IdentifierName("SimpleAdapter"));
        A.CallTo(() => _blueprint.NestedAdapters).Returns(ImmutableList.Create(_nestedAdapter));
        A.CallTo(() => _nestedAdapter.FieldName).Returns("_nestedAdapter");
        A.CallTo(() => _nestedAdapter.InterfaceType).Returns(
          GenericName(Identifier("NestedAdapter"),
            TypeArgumentList(SingletonSeparatedList<TypeSyntax>(IdentifierName("Banana")))));
      }

      [Test]
      public void ShouldGenerateSetupMethod() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateCorrectSetupMethod() {
        TestUtilities.FormatCode(_sut.Generate(_context)).ShouldBeSameCodeAs(@"[SetUp]
public virtual void SetUp()
{
    _nestedAdapter = _nestedAdapter.Fake();
    _sut = new SimpleAdapter(_nestedAdapter);
}");
      }
    }
  }
}