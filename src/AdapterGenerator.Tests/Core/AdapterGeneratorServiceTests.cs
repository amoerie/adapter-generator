using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Core.Writing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core {
  [TestFixture]
  public class AdapterGeneratorServiceTests {
    AdapterGeneratorService _sut;
    private ILogger _logger;
    private IFileParser _fileParser;
    private IAdaptersGenerator _adaptersGenerator;
    private IAdapterWriter _adapterWriter;
    private IMatcher _matcher;
    private IAdapterTestsGenerator _adapterTestsGenerator;
    private IAdapterTestsWriter _adapterTestsWriter;

    [SetUp]
    public virtual void SetUp() {
      _logger = _logger.Fake();
      _fileParser = _fileParser.Fake();
      _matcher = _matcher.Fake();
      _adaptersGenerator = _adaptersGenerator.Fake();
      _adapterWriter = _adapterWriter.Fake();
      _adapterTestsGenerator = _adapterTestsGenerator.Fake();
      _adapterTestsWriter = _adapterTestsWriter.Fake();
      _sut = new AdapterGeneratorService(_logger, _fileParser, _matcher, _adaptersGenerator, _adapterWriter,
        _adapterTestsGenerator, _adapterTestsWriter);
    }

    [TestFixture]
    public class Constructor : AdapterGeneratorServiceTests {
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
    public class GenerateAdapters : AdapterGeneratorServiceTests {
      private IImmutableList<FileInfo> _sourceFiles;
      private IImmutableList<FileInfo> _targetFiles;
      private DirectoryInfo _outputDirectory;
      private FileInfo _source;
      private FileInfo _target;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = new FileInfo("abc");
        _sourceFiles = ImmutableList.Create(_source);
        _target = new FileInfo("def");
        _targetFiles = ImmutableList.Create(_target);
        _outputDirectory = new DirectoryInfo("blablabla");
      }

      [Test]
      public void HappyCase() {
        var sourceTypes = A.Fake<ITypeDeclarations>();
        var targetTypes = A.Fake<ITypeDeclarations>();
        A.CallTo(() => _fileParser.ParseFiles(A<IEnumerable<FileInfo>>.That.IsSameSequenceAs(new[] {_source})))
          .Returns(sourceTypes);
        A.CallTo(() => _fileParser.ParseFiles(A<IEnumerable<FileInfo>>.That.IsSameSequenceAs(new[] {_target})))
          .Returns(targetTypes);
        IMatches matches = A.Fake<IMatches>();
        A.CallTo(() => _matcher.Match(sourceTypes, targetTypes)).Returns(matches);
        IImmutableList<IGeneratedAdapter> generatedAdapters = ImmutableList.Create<IGeneratedAdapter>();
        A.CallTo(() => _adaptersGenerator.Generate(sourceTypes, targetTypes, matches))
          .Returns(generatedAdapters);
        _sut.GenerateAdapters(_sourceFiles, _targetFiles, _outputDirectory);
        A.CallTo(() => _adapterWriter.Write(generatedAdapters, _outputDirectory)).MustHaveHappened();
      }
    }
  }
}