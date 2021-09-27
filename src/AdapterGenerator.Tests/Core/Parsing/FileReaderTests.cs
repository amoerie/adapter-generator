using System.IO;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Parsing {
  [TestFixture]
  public class FileReaderTests {
    private FileReader _sut;
    private FileInfo _helloWorldFile;

    [SetUp]
    public virtual void SetUp() {
      _sut = new FileReader(new ConsoleLogger());
      _helloWorldFile = TestDataIndex.Core.HelloWorldTxt;
    }

    [TestFixture]
    public class Constructor : FileReaderTests {
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
    public class ReadFile : FileReaderTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldReadHelloWorld() {
        _sut.ReadFile(_helloWorldFile.FullName).Should().Be("Hello world!");
      }
    }
  }
}