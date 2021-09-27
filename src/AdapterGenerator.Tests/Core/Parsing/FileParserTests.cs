using System.IO;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Parsing {
  [TestFixture]
  public class FileParserTests {
    FileParser _sut;
    private IFileReader _fileReader;
    private ICodeToSyntaxTreeParser _codeParser;
    private ITypeDeclarationsFromSyntaxTreeExtractor _typeDeclarationsExtractor;

    [SetUp]
    public virtual void SetUp() {
      _fileReader = _fileReader.Fake();
      _codeParser = _codeParser.Fake();
      _typeDeclarationsExtractor = _typeDeclarationsExtractor.Fake();
      _sut = new FileParser(_fileReader, _codeParser, _typeDeclarationsExtractor);
    }

    [TestFixture]
    public class Constructor : FileParserTests {
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
    public class ParseFiles : FileParserTests {
      private FileInfo _file;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _file = new FileInfo("abc");
      }

      [Test]
      public void HappyCase() {
        A.CallTo(() => _fileReader.ReadFile(_file.FullName)).Returns("hello world");
        var syntaxTree = A.Fake<SyntaxTree>();
        A.CallTo(() => _codeParser.Parse("hello world")).Returns(syntaxTree);
        var declarations = A.Fake<ITypeDeclarations>();
        A.CallTo(() => _typeDeclarationsExtractor.Extract(syntaxTree)).Returns(declarations);
        _sut.ParseFiles(new[] {_file}).Should().Be(declarations);
      }
    }
  }
}