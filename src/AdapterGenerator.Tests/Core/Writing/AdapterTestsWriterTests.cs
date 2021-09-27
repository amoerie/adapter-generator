using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Writing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Writing {
  [TestFixture]
  public class AdapterTestsWriterTests {
    AdapterTestsWriter _sut;
    private IFileWriter _fileWriter;
    private ISyntaxFormatter _formatter;

    [SetUp]
    public virtual void SetUp() {
      _fileWriter = _fileWriter.Fake();
      _formatter = _formatter.Fake();
      _sut = new AdapterTestsWriter(_fileWriter, _formatter);
    }

    [TestFixture]
    public class Constructor : AdapterTestsWriterTests {
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
    public class WriteAdapters : AdapterTestsWriterTests {
      private IImmutableList<IGeneratedAdapterTests> _generatedAdapterTests;
      private DirectoryInfo _outputDirectory;
      private IGeneratedAdapterTests _generatedAdapterTest;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _generatedAdapterTest = A.Fake<IGeneratedAdapterTests>();
        _outputDirectory = new DirectoryInfo(".");
        _generatedAdapterTests = ImmutableList.Create(_generatedAdapterTest);
        A.CallTo(() => _generatedAdapterTest.TestClassName).Returns("GeneratedAdapterTests");
      }

      [Test]
      public void ShouldWriteItToFile() {
        var path = Path.Combine(_outputDirectory.FullName, "GeneratedAdapterTests.cs");
        var code = "abc";
        A.CallTo(() => _formatter.Format(_generatedAdapterTest.CompilationUnitSyntax)).Returns(code);
        _sut.Write(_generatedAdapterTests, _outputDirectory);
        A.CallTo(() => _fileWriter.WriteFile(path, code)).MustHaveHappened();
      }
    }
  }
}