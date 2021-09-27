using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Parsing {
  [TestFixture]
  public class TypeDeclarationsFromSyntaxTreeExtractorTests {
    TypeDeclarationsFromSyntaxTreeExtractor _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new TypeDeclarationsFromSyntaxTreeExtractor(new ConsoleLogger(), new NamespaceDeclarationSyntaxFinder());
    }

    [TestFixture]
    public class Constructor : TypeDeclarationsFromSyntaxTreeExtractorTests {
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
    public class Extract : TypeDeclarationsFromSyntaxTreeExtractorTests {
      private SyntaxTree _syntaxTree;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _syntaxTree = new CodeToSyntaxTreeParser(new ConsoleLogger())
          .Parse(new FileReader(new ConsoleLogger()).ReadFile(TestDataIndex.Core.MultipleClassesInOneFile.FullName));
      }

      [Test]
      public void ShouldFindClassDeclaration() {
        var declarations = _sut.Extract(_syntaxTree);
        var classes = declarations.Classes;
        classes.Should().HaveCount(3);
        classes.Should().Contain(c => c.ClassDeclarationSyntax.Identifier.Text == "FindPersonResponse");
        classes.Should().Contain(c => c.ClassDeclarationSyntax.Identifier.Text == "Person");
        classes.Should().Contain(c => c.ClassDeclarationSyntax.Identifier.Text == "Address");
      }

      [Test]
      public void ShouldFindEnums() {
        var declarations = _sut.Extract(_syntaxTree);
        var enums = declarations.Enums;
        enums.Should().HaveCount(1);
        enums.Should().Contain(e => e.EnumDeclarationSyntax.Identifier.Text == "Gender");
      }
    }
  }
}