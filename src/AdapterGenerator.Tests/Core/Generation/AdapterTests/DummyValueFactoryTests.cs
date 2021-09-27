using System;
using System.Collections.Immutable;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests {
  [TestFixture]
  public class DummyValueFactoryTests {
    DummyValueFactory _sut;
    ITypeDeclarations _typeDeclarations;

    [SetUp]
    public virtual void SetUp() {
      _typeDeclarations = _typeDeclarations.Fake();
      A.CallTo(() => _typeDeclarations.Classes).Returns(ImmutableList<IClass>.Empty);
      A.CallTo(() => _typeDeclarations.Enums).Returns(ImmutableList<IEnum>.Empty);
      _sut = new DummyValueFactory();
    }

    [TestFixture]
    public class Constructor : DummyValueFactoryTests {
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
    public class CreateDummyValue : DummyValueFactoryTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      private void AssertDummyValue(TypeSyntax type, string expectedCode) {
        // when this issue is fixed: https://github.com/dotnet/roslyn/issues/11642
        // you can remove this workaround 
        // and replace it with TestUtilities.FormatCode(_sut.CreateDummyValue(type)).ShouldBeSameCodeAs(expectedCode);
        var dummyValue = _sut.CreateDummyValue(_typeDeclarations, type);
        var compilationUnit = CompilationUnit()
          .AddMembers(
            MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), "Dummy")
              .WithBody(Block().AddStatements(
                LocalDeclarationStatement(
                  VariableDeclaration(
                    IdentifierName("var"))
                    .WithVariables(
                      SingletonSeparatedList(
                        VariableDeclarator(
                          Identifier("dummyValue"))
                          .WithInitializer(EqualsValueClause(dummyValue))))))));
        var code = TestUtilities.FormatCode(compilationUnit);
        var dummyValueOnly = code
          .Replace($"void Dummy(){Environment.NewLine}{{{Environment.NewLine}    var dummyValue = ", "")
          .Replace($";{Environment.NewLine}}}", "");
        dummyValueOnly.ShouldBeSameCodeAs(expectedCode);
      }

      [TestCase(SyntaxKind.StringKeyword, "\"abc\"")]
      [TestCase(SyntaxKind.DoubleKeyword, "42")]
      [TestCase(SyntaxKind.IntKeyword, "42")]
      [TestCase(SyntaxKind.LongKeyword, "42")]
      [TestCase(SyntaxKind.DecimalKeyword, "42")]
      [TestCase(SyntaxKind.CharKeyword, "'x'")]
      public void ShouldCreateCorrectValueForPredefinedTypes(SyntaxKind predefinedTypeKind, string expectedCode) {
        AssertDummyValue(PredefinedType(Token(predefinedTypeKind)), expectedCode);
      }

      [TestCase(SyntaxKind.StringKeyword, "new string[] { \"abc\", \"abc\", \"abc\" }")]
      [TestCase(SyntaxKind.DoubleKeyword, "new double[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.IntKeyword, "new int[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.LongKeyword, "new long[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.CharKeyword, "new char[] { 'x', 'x', 'x' }")]
      public void ShouldCreateCorrectValueForArrayOfPredefinedTypes(SyntaxKind predefinedTypeKind, string expectedCode) {
        AssertDummyValue(ArrayType(PredefinedType(Token(predefinedTypeKind))), expectedCode);
      }

      [TestCase(SyntaxKind.StringKeyword, "new string[] { \"abc\", \"abc\", \"abc\" }")]
      [TestCase(SyntaxKind.DoubleKeyword, "new double[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.IntKeyword, "new int[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.LongKeyword, "new long[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.CharKeyword, "new char[] { 'x', 'x', 'x' }")]
      public void ShouldCreateCorrectValueForIEnumerableOfPredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(
          GenericName("IEnumerable").AddTypeArgumentListArguments(PredefinedType(Token(predefinedTypeKind))),
          expectedCode);
      }

      [TestCase(SyntaxKind.StringKeyword, "new string[] { \"abc\", \"abc\", \"abc\" }.ToList()")]
      [TestCase(SyntaxKind.DoubleKeyword, "new double[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.IntKeyword, "new int[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.LongKeyword, "new long[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.CharKeyword, "new char[] { 'x', 'x', 'x' }.ToList()")]
      public void ShouldCreateCorrectValueForICollectionOfPredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(
          GenericName("ICollection").AddTypeArgumentListArguments(PredefinedType(Token(predefinedTypeKind))),
          expectedCode);
      }

      [TestCase(SyntaxKind.StringKeyword, "new string[] { \"abc\", \"abc\", \"abc\" }.ToList()")]
      [TestCase(SyntaxKind.DoubleKeyword, "new double[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.IntKeyword, "new int[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.LongKeyword, "new long[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.CharKeyword, "new char[] { 'x', 'x', 'x' }.ToList()")]
      public void ShouldCreateCorrectValueForIListOfPredefinedTypes(SyntaxKind predefinedTypeKind, string expectedCode) {
        AssertDummyValue(GenericName("IList").AddTypeArgumentListArguments(PredefinedType(Token(predefinedTypeKind))),
          expectedCode);
      }

      [TestCase(SyntaxKind.StringKeyword, "\"abc\"")]
      [TestCase(SyntaxKind.DoubleKeyword, "42")]
      [TestCase(SyntaxKind.IntKeyword, "42")]
      [TestCase(SyntaxKind.LongKeyword, "42")]
      [TestCase(SyntaxKind.DecimalKeyword, "42")]
      [TestCase(SyntaxKind.CharKeyword, "'x'")]
      public void ShouldCreateCorrectValueForNullablePredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(NullableType(PredefinedType(Token(predefinedTypeKind))), expectedCode);
      }

      [TestCase(SyntaxKind.DoubleKeyword, "new double?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.IntKeyword, "new int?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.LongKeyword, "new long?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.CharKeyword, "new char?[] { 'x', 'x', 'x' }")]
      public void ShouldCreateCorrectValueForArrayOfNullablePredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(ArrayType(NullableType(PredefinedType(Token(predefinedTypeKind)))), expectedCode);
      }

      [TestCase(SyntaxKind.DoubleKeyword, "new double?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.IntKeyword, "new int?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.LongKeyword, "new long?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal?[] { 42, 42, 42 }")]
      [TestCase(SyntaxKind.CharKeyword, "new char?[] { 'x', 'x', 'x' }")]
      public void ShouldCreateCorrectValueForIEnumerableOfNullablePredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(
          GenericName("IEnumerable")
            .AddTypeArgumentListArguments(NullableType(PredefinedType(Token(predefinedTypeKind)))),
          expectedCode);
      }

      [TestCase(SyntaxKind.DoubleKeyword, "new double?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.IntKeyword, "new int?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.LongKeyword, "new long?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.CharKeyword, "new char?[] { 'x', 'x', 'x' }.ToList()")]
      public void ShouldCreateCorrectValueForICollectionOfNullablePredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(
          GenericName("ICollection")
            .AddTypeArgumentListArguments(NullableType(PredefinedType(Token(predefinedTypeKind)))),
          expectedCode);
      }

      [TestCase(SyntaxKind.DoubleKeyword, "new double?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.IntKeyword, "new int?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.LongKeyword, "new long?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.DecimalKeyword, "new decimal?[] { 42, 42, 42 }.ToList()")]
      [TestCase(SyntaxKind.CharKeyword, "new char?[] { 'x', 'x', 'x' }.ToList()")]
      public void ShouldCreateCorrectValueForIListOfNullablePredefinedTypes(SyntaxKind predefinedTypeKind,
        string expectedCode) {
        AssertDummyValue(
          GenericName("IList").AddTypeArgumentListArguments(NullableType(PredefinedType(Token(predefinedTypeKind)))),
          expectedCode);
      }


      [TestCase("DateTime", "new DateTime(1990, 6, 2)")]
      [TestCase("TimeSpan", "new TimeSpan(1, 2, 3, 4, 5)")]
      public void ShouldCreateCorrectValueForKnownIdentifiers(string identifierName, string expectedCode) {
        AssertDummyValue(IdentifierName(identifierName), expectedCode);
      }

      [TestCase("DateTime", "new DateTime(1990, 6, 2)")]
      [TestCase("TimeSpan", "new TimeSpan(1, 2, 3, 4, 5)")]
      public void ShouldCreateCorrectValueForNullableKnownIdentifiers(string identifierName, string expectedCode) {
        AssertDummyValue(NullableType(IdentifierName(identifierName)), expectedCode);
      }

      [TestCase("DateTime", "new DateTime[] { new DateTime(1990, 6, 2), new DateTime(1990, 6, 2), new DateTime(1990, 6, 2) }")]
      [TestCase("TimeSpan", "new TimeSpan[] { new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(1, 2, 3, 4, 5) }")]
      public void ShouldCreateCorrectValueForArrayOfKnownIdentifiers(string identifierName, string expectedCode) {
        AssertDummyValue(ArrayType(IdentifierName(identifierName)), expectedCode);
      }

      [TestCase("DateTime", "new DateTime?[] { new DateTime(1990, 6, 2), new DateTime(1990, 6, 2), new DateTime(1990, 6, 2) }")]
      [TestCase("TimeSpan", "new TimeSpan?[] { new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(1, 2, 3, 4, 5) }")]
      public void ShouldCreateCorrectValueForArrayOfNullableKnownIdentifiers(string identifierName, string expectedCode) {
        AssertDummyValue(ArrayType(NullableType(IdentifierName(identifierName))), expectedCode);
      }

      [Test]
      public void ShouldCreateCorrectValueForEnumType() {
        var enumType = TestUtilities.ParseEnum("public enum Gender { Male, Female }");
        A.CallTo(() => _typeDeclarations.Enums).Returns(ImmutableList.Create(enumType));
        AssertDummyValue(IdentifierName("Gender"), "Gender.Male");
      }

      [Test]
      public void ShouldCreateCorrectValueForArrayOfEnumTypes() {
        var enumType = TestUtilities.ParseEnum("public enum Gender { Male, Female }");
        A.CallTo(() => _typeDeclarations.Enums).Returns(ImmutableList.Create(enumType));
        AssertDummyValue(ArrayType(IdentifierName("Gender")), "new Gender[] { Gender.Male, Gender.Male, Gender.Male }");
      }

      [Test]
      public void ShouldCreateCorrectValueForNullableEnumType() {
        var enumType = TestUtilities.ParseEnum("public enum Gender { Male, Female }");
        A.CallTo(() => _typeDeclarations.Enums).Returns(ImmutableList.Create(enumType));
        AssertDummyValue(NullableType(IdentifierName("Gender")), "Gender.Male");
      }

      [Test]
      public void ShouldCreateCorrectValueForArrayOfNullableEnumType() {
        var enumType = TestUtilities.ParseEnum("public enum Gender { Male, Female }");
        A.CallTo(() => _typeDeclarations.Enums).Returns(ImmutableList.Create(enumType));
        AssertDummyValue(ArrayType(NullableType(IdentifierName("Gender"))), "new Gender?[] { Gender.Male, Gender.Male, Gender.Male }");
      }

      [Test]
      public void ShouldCreateCorrectValueForClassType() {
        var classType = TestUtilities.ParseClass("public class Trainer {}");
        A.CallTo(() => _typeDeclarations.Classes).Returns(ImmutableList.Create(classType));
        AssertDummyValue(IdentifierName("Trainer"), "new Trainer()");
      }

      [Test]
      public void ShouldCreateCorrectValueForArrayOfClassTypes() {
        var classType = TestUtilities.ParseClass("public class Trainer {}");
        A.CallTo(() => _typeDeclarations.Classes).Returns(ImmutableList.Create(classType));
        AssertDummyValue(ArrayType(IdentifierName("Trainer")), "new Trainer[] { new Trainer(), new Trainer(), new Trainer() }");
      }
    }
  }
}