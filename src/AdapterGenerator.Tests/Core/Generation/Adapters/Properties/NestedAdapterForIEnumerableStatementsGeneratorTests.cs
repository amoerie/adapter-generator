using System.Linq;
using AdapterGenerator.Core.Generation;
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
  public class NestedAdapterForIEnumerableStatementsGeneratorTests {
    NestedAdapterForIEnumerableStatementsGenerator _sut;
    private IClassAdapterGenerationContextWithClass _context;
    private IPropertyMatch _nestedMatch;
    private IAdapterBlueprint _nestedAdapter;
    private ITypeSyntaxAnalyzer _typeSyntaxAnalyzer;


    [SetUp]
    public virtual void SetUp() {
      var targetClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
      var targetNestedProperty = targetClass.FindPropertyByName("Nesteds");
      var sourceClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
      var sourceNestedProperty = sourceClass.FindPropertyByName("Nesteds");
      _nestedMatch = new PropertyMatch {Source = sourceNestedProperty, Target = targetNestedProperty};
      _context = _context.Fake();
      _nestedAdapter = _nestedAdapter.Fake();
      _typeSyntaxAnalyzer = _typeSyntaxAnalyzer.Fake();
      _sut = new NestedAdapterForIEnumerableStatementsGenerator(_context, _nestedMatch, _nestedAdapter,
        _typeSyntaxAnalyzer);
    }

    [TestFixture]
    public class Constructor : NestedAdapterForIEnumerableStatementsGeneratorTests {
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
    public class GenerateStatements : NestedAdapterForIEnumerableStatementsGeneratorTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldGenerateCorrectStatementForIEnumerable() {
        A.CallTo(() => _nestedAdapter.FieldName).Returns("_nestedAdapter");
        A.CallTo(() => _typeSyntaxAnalyzer.IsIEnumerable(_nestedMatch.Target.PropertyDeclarationSyntax.Type))
          .Returns(true);
        A.CallTo(() => _typeSyntaxAnalyzer.IsArray(_nestedMatch.Target.PropertyDeclarationSyntax.Type)).Returns(false);
        A.CallTo(() => _typeSyntaxAnalyzer.IsCollectionType(_nestedMatch.Target.PropertyDeclarationSyntax.Type))
          .Returns(false);
        TestUtilities.FormatCode(_sut.Generate().Single())
          .ShouldBeSameCodeAs("target.Nesteds = source.Nesteds?.Select(_nestedAdapter.Adapt);");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForArray() {
        A.CallTo(() => _nestedAdapter.FieldName).Returns("_nestedAdapter");
        A.CallTo(() => _typeSyntaxAnalyzer.IsIEnumerable(_nestedMatch.Target.PropertyDeclarationSyntax.Type))
          .Returns(false);
        A.CallTo(() => _typeSyntaxAnalyzer.IsArray(_nestedMatch.Target.PropertyDeclarationSyntax.Type)).Returns(true);
        A.CallTo(() => _typeSyntaxAnalyzer.IsCollectionType(_nestedMatch.Target.PropertyDeclarationSyntax.Type))
          .Returns(false);
        TestUtilities.FormatCode(_sut.Generate().Single())
          .ShouldBeSameCodeAs("target.Nesteds = source.Nesteds?.Select(_nestedAdapter.Adapt).ToArray();");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForCollection() {
        A.CallTo(() => _nestedAdapter.FieldName).Returns("_nestedAdapter");
        A.CallTo(() => _typeSyntaxAnalyzer.IsIEnumerable(_nestedMatch.Target.PropertyDeclarationSyntax.Type))
          .Returns(false);
        A.CallTo(() => _typeSyntaxAnalyzer.IsArray(_nestedMatch.Target.PropertyDeclarationSyntax.Type)).Returns(false);
        A.CallTo(() => _typeSyntaxAnalyzer.IsCollectionType(_nestedMatch.Target.PropertyDeclarationSyntax.Type))
          .Returns(true);
        TestUtilities.FormatCode(_sut.Generate().Single())
          .ShouldBeSameCodeAs("target.Nesteds = source.Nesteds?.Select(_nestedAdapter.Adapt).ToList();");
      }
    }
  }
}