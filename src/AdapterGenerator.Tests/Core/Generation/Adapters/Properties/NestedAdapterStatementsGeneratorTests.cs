using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.Properties {
  [TestFixture]
  public class NestedAdapterStatementsGeneratorTests {
    NestedAdapterStatementsGenerator _sut;
    private IClassAdapterGenerationContextWithClass _context;
    private IPropertyMatch _nestedMatch;
    private IAdapterBlueprint _nestedAdapter;


    [SetUp]
    public virtual void SetUp() {
      var targetClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
      var targetNestedProperty = targetClass.FindPropertyByName("Nested");
      var sourceClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
      var sourceNestedProperty = sourceClass.FindPropertyByName("Nested");
      _nestedMatch = new PropertyMatch {Source = sourceNestedProperty, Target = targetNestedProperty};
      _context = _context.Fake();
      _nestedAdapter = _nestedAdapter.Fake();
      _sut = new NestedAdapterStatementsGenerator(_context, _nestedMatch, _nestedAdapter);
    }

    [TestFixture]
    public class Constructor : NestedAdapterStatementsGeneratorTests {
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
    public class GenerateStatements : NestedAdapterStatementsGeneratorTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldGenerateCorrectStatementForStringMatch() {
        A.CallTo(() => _nestedAdapter.FieldName).Returns("_nestedAdapter");
        TestUtilities.FormatCode(_sut.Generate().Single())
          .ShouldBeSameCodeAs("target.Nested = _nestedAdapter.Adapt(source.Nested);");
      }
    }
  }
}