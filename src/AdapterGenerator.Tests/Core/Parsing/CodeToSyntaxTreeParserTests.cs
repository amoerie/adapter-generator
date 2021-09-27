using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Parsing {
  [TestFixture]
  public class CodeToSyntaxTreeParserTests {
    CodeToSyntaxTreeParser _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new CodeToSyntaxTreeParser(new ConsoleLogger());
    }

    [TestFixture]
    public class Constructor : CodeToSyntaxTreeParserTests {
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
    public class Parse : CodeToSyntaxTreeParserTests {
      private string _code;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _code = new FileReader(new ConsoleLogger()).ReadFile(TestDataIndex.Core.OneClass.FullName);
      }

      [Test]
      public void ShouldReturnSyntaxTreeWithARoot() {
        var syntaxTree = _sut.Parse(_code);
        var root = syntaxTree.GetCompilationUnitRoot();
        root.Should().NotBeNull();
      }
    }
  }
}