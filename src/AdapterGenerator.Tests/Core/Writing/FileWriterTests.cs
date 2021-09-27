using System;
using System.IO;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Core.Writing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Writing {
  [TestFixture]
  public class FileWriterTests {
    FileWriter _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new FileWriter(new ConsoleLogger());
    }

    [TestFixture]
    public class Constructor : FileWriterTests {
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
    public class WriteFile : FileWriterTests {
      private string _testDataFilePath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _testDataFilePath = Path.Combine(TestDataIndex.RootFolder.FullName, "FileWriter.txt");
      }

      [TearDown]
      public void TearDown() {
        Console.WriteLine("Deleting " + _testDataFilePath);
        File.Delete(_testDataFilePath);
      }

      [Test]
      public void ShouldCreateFileAndWriteIt() {
        _sut.WriteFile(_testDataFilePath, "Hello world!");
        new FileReader(new ConsoleLogger()).ReadFile(_testDataFilePath).Should().Be("Hello world!");
      }
    }
  }
}