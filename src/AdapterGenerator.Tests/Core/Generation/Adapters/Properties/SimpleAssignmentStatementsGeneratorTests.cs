using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.Properties {
  [TestFixture]
  public class SimpleAssignmentStatementsGeneratorTests {
    private IClassAdapterGenerationContextWithClass _context;
    private IPropertyMatch _textMatch;
    private IPropertyMatch _dateMatch;
    private IPropertyMatch _nullableDateMatch;
    private IPropertyMatch _numbersMatch;
    SimpleAssignmentStatementsGenerator _sut;

    [SetUp]
    public virtual void SetUp() {
      _context = _context.Fake();
      var targetClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
      var targetTextProperty = targetClass.FindPropertyByName("Text");
      var targetDateProperty = targetClass.FindPropertyByName("Date");
      var targetNullableDateProperty = targetClass.FindPropertyByName("NullableDate");
      var targetNumbersProperty = targetClass.FindPropertyByName("Numbers");
      var sourceClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
      var sourceTextProperty = sourceClass.FindPropertyByName("Text");
      var sourceDateProperty = sourceClass.FindPropertyByName("Date");
      var sourceNullableDateProperty = sourceClass.FindPropertyByName("NullableDate");
      var sourceNumbersProperty = sourceClass.FindPropertyByName("Numbers");

      _textMatch = new PropertyMatch {Source = sourceTextProperty, Target = targetTextProperty};
      _dateMatch = new PropertyMatch {Source = sourceDateProperty, Target = targetDateProperty};
      _nullableDateMatch = new PropertyMatch {
        Source = sourceNullableDateProperty,
        Target = targetNullableDateProperty
      };
      _numbersMatch = new PropertyMatch {Source = sourceNumbersProperty, Target = targetNumbersProperty};
      _sut = new SimpleAssignmentStatementsGenerator(_context, _numbersMatch);
    }

    [TestFixture]
    public class Constructor : SimpleAssignmentStatementsGeneratorTests {
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
    public class GenerateStatements : SimpleAssignmentStatementsGeneratorTests {
      [Test]
      public void ShouldGenerateCorrectStatementForStringMatch() {
        TestUtilities.FormatCode(new SimpleAssignmentStatementsGenerator(_context, _textMatch).Generate().Single())
          .ShouldBeSameCodeAs("target.Text = source.Text;");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForDateMatch() {
        TestUtilities.FormatCode(new SimpleAssignmentStatementsGenerator(_context, _dateMatch).Generate().Single())
          .ShouldBeSameCodeAs("target.Date = source.Date;");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForNullableDateMatch() {
        TestUtilities.FormatCode(
          new SimpleAssignmentStatementsGenerator(_context, _nullableDateMatch).Generate().Single())
          .ShouldBeSameCodeAs("target.NullableDate = source.NullableDate;");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForNullableDateAndDateMatch() {
        TestUtilities.FormatCode(new SimpleAssignmentStatementsGenerator(_context, new PropertyMatch {
          Source = _dateMatch.Source,
          Target = _nullableDateMatch.Target
        }).Generate().Single())
          .ShouldBeSameCodeAs("target.NullableDate = source.Date;");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForNumbersMatch() {
        TestUtilities.FormatCode(new SimpleAssignmentStatementsGenerator(_context, _numbersMatch).Generate().Single())
          .ShouldBeSameCodeAs("target.Numbers = source.Numbers;");
      }
    }
  }
}