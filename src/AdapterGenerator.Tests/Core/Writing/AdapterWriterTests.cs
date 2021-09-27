using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Writing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Writing {
  [TestFixture]
  public class AdapterWriterTests {
    AdapterWriter _sut;
    private IFileWriter _fileWriter;
    private ISyntaxFormatter _formatter;

    [SetUp]
    public virtual void SetUp() {
      _fileWriter = _fileWriter.Fake();
      _formatter = _formatter.Fake();
      _sut = new AdapterWriter(_fileWriter, _formatter);
    }

    [TestFixture]
    public class Constructor : AdapterWriterTests {
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
    public class WriteAdapters : AdapterWriterTests {
      private IImmutableList<IGeneratedAdapter> _generatedAdapters;
      private DirectoryInfo _outputDirectory;
      private IGeneratedAdapter _generatedAdapter;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _generatedAdapter = A.Fake<IGeneratedAdapter>();
        _outputDirectory = new DirectoryInfo(".");
        _generatedAdapters = ImmutableList.Create(_generatedAdapter);
        A.CallTo(() => _generatedAdapter.Blueprint.Name).Returns(SyntaxFactory.IdentifierName("GeneratedAdapter"));
      }

      [Test]
      public void ShouldWriteItToFile() {
        var path = Path.Combine(_outputDirectory.FullName, "GeneratedAdapter.cs");
        var code = "abc";
        A.CallTo(() => _formatter.Format(_generatedAdapter.CompilationUnitSyntax)).Returns(code);
        _sut.Write(_generatedAdapters, _outputDirectory);
        A.CallTo(() => _fileWriter.WriteFile(path, code)).MustHaveHappened();
      }
    }
  }
}