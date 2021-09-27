using System;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.Properties {
  [TestFixture]
  public class UnmatchedPropertyStatementsGeneratorTests {
    AssignDefaultValueStatementsGenerator _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new AssignDefaultValueStatementsGenerator(A.Fake<IClassAdapterGenerationContextWithClass>(),
        A.Fake<IProperty>());
    }

    [TestFixture]
    public class Constructor : UnmatchedPropertyStatementsGeneratorTests {
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
    public class GenerateStatements : UnmatchedPropertyStatementsGeneratorTests {
      private IClassAdapterGenerationContextWithClass _context;
      private IProperty _textProperty;
      private IProperty _dateProperty;
      private IProperty _numbersProperty;
      private IProperty _nullableDateProperty;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        var targetClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
        _textProperty = targetClass.FindPropertyByName("Text");
        _dateProperty = targetClass.FindPropertyByName("Date");
        _nullableDateProperty = targetClass.FindPropertyByName("NullableDate");
        _numbersProperty = targetClass.FindPropertyByName("Numbers");
      }

      [Test]
      public void ShouldThrowWhenContextIsNull() {
        new Action(() => new AssignDefaultValueStatementsGenerator(_context = null, _numbersProperty).Generate())
          .ShouldThrow<ArgumentNullException>
          ();
      }

      [Test]
      public void ShouldThrowWhenPropertyIsNull() {
        new Action(() => new AssignDefaultValueStatementsGenerator(_context, _numbersProperty = null).Generate())
          .ShouldThrow<ArgumentNullException>
          ();
      }

      [Test]
      public void ShouldGenerateCorrectStatementForStringProperty() {
        TestUtilities.FormatCode(new AssignDefaultValueStatementsGenerator(_context, _textProperty).Generate().Single())
          .ShouldBeSameCodeAs("target.Text = default(string);// TODO adapt this manually");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForDateProperty() {
        TestUtilities.FormatCode(new AssignDefaultValueStatementsGenerator(_context, _dateProperty).Generate().Single())
          .ShouldBeSameCodeAs("target.Date = default(DateTime);// TODO adapt this manually");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForNullableDateProperty() {
        TestUtilities.FormatCode(
          new AssignDefaultValueStatementsGenerator(_context, _nullableDateProperty).Generate().Single())
          .ShouldBeSameCodeAs("target.NullableDate = default(DateTime?);// TODO adapt this manually");
      }

      [Test]
      public void ShouldGenerateCorrectStatementForNumbersProperty() {
        TestUtilities.FormatCode(
          new AssignDefaultValueStatementsGenerator(_context, _numbersProperty).Generate().Single())
          .ShouldBeSameCodeAs("target.Numbers = default(IEnumerable<int>);// TODO adapt this manually");
      }
    }
  }
}