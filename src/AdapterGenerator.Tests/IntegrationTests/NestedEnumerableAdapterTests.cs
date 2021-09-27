using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using Autofac;
using NUnit.Framework;

namespace AdapterGenerator.Tests.IntegrationTests {
  [TestFixture]
  public class NestedEnumerableAdapterTests {
    IAdapterGeneratorService _adapterGeneratorService;

    [SetUp]
    public virtual void SetUp() {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      _adapterGeneratorService = container.Resolve<IAdapterGeneratorService>();
    }

    [Test]
    public void ShouldGenerateAdapterForASimpleClassWithOnePredefinedProperty() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceSpeaker);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetSpeaker);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "SpeakerAdapter.cs"));
      var expectedFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class SpeakerAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Speaker, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Speaker>
    {
        private readonly IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session> _sessionAdapter;
        public SpeakerAdapter(IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session> sessionAdapter)
        {
            if (sessionAdapter == null)
                throw new ArgumentNullException(nameof(sessionAdapter));
            _sessionAdapter = sessionAdapter;
        }

        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Speaker Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Speaker source)
        {
            if (source == null)
                return null;
            var target = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Speaker();
            target.EnumerableOfSessions = source.EnumerableOfSessions?.Select(_sessionAdapter.Adapt);
            target.ListOfSessions = source.ListOfSessions?.Select(_sessionAdapter.Adapt).ToList();
            target.CollectionOfSessions = source.CollectionOfSessions?.Select(_sessionAdapter.Adapt).ToList();
            target.ArrayOfSessions = source.ArrayOfSessions?.Select(_sessionAdapter.Adapt).ToArray();
            return target;
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateAdapterTestsForASimpleClassWithOnePredefinedProperty() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceSpeaker);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetSpeaker);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "SpeakerAdapterTests.cs"));
      var expectedFileContents =
        @"using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    [TestFixture]
    public class SpeakerAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.SpeakerAdapter _sut;
        private IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session> _sessionAdapter;
        [SetUp]
        public virtual void SetUp()
        {
            _sessionAdapter = _sessionAdapter.Fake();
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.SpeakerAdapter(_sessionAdapter);
        }

        [TestFixture]
        public class Constructor : SpeakerAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : SpeakerAdapterTests
        {
            private AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Speaker _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Speaker();
            }

            [Test]
            public void ShouldReturnNullWhenSourceIsNull()
            {
                _source = null;
                var target = _sut.Adapt(_source);
                target.Should().BeNull();
            }

            [Test]
            public void ShouldAdaptEnumerableOfSessions()
            {
                _source.EnumerableOfSessions = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session()};
                var expected = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session()};
                A.CallTo(() => _sessionAdapter.Adapt(_source.EnumerableOfSessions.ElementAt(0))).Returns(expected.ElementAt(0));
                A.CallTo(() => _sessionAdapter.Adapt(_source.EnumerableOfSessions.ElementAt(1))).Returns(expected.ElementAt(1));
                A.CallTo(() => _sessionAdapter.Adapt(_source.EnumerableOfSessions.ElementAt(2))).Returns(expected.ElementAt(2));
                var target = _sut.Adapt(_source);
                target.EnumerableOfSessions.Should().HaveCount(3);
                target.EnumerableOfSessions.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
                target.EnumerableOfSessions.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
                target.EnumerableOfSessions.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
            }

            [Test]
            public void ShouldAdaptListOfSessions()
            {
                _source.ListOfSessions = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session()};
                var expected = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session()}.ToList();
                A.CallTo(() => _sessionAdapter.Adapt(_source.ListOfSessions.ElementAt(0))).Returns(expected.ElementAt(0));
                A.CallTo(() => _sessionAdapter.Adapt(_source.ListOfSessions.ElementAt(1))).Returns(expected.ElementAt(1));
                A.CallTo(() => _sessionAdapter.Adapt(_source.ListOfSessions.ElementAt(2))).Returns(expected.ElementAt(2));
                var target = _sut.Adapt(_source);
                target.ListOfSessions.Should().HaveCount(3);
                target.ListOfSessions.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
                target.ListOfSessions.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
                target.ListOfSessions.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
            }

            [Test]
            public void ShouldAdaptCollectionOfSessions()
            {
                _source.CollectionOfSessions = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session()}.ToList();
                var expected = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session()}.ToList();
                A.CallTo(() => _sessionAdapter.Adapt(_source.CollectionOfSessions.ElementAt(0))).Returns(expected.ElementAt(0));
                A.CallTo(() => _sessionAdapter.Adapt(_source.CollectionOfSessions.ElementAt(1))).Returns(expected.ElementAt(1));
                A.CallTo(() => _sessionAdapter.Adapt(_source.CollectionOfSessions.ElementAt(2))).Returns(expected.ElementAt(2));
                var target = _sut.Adapt(_source);
                target.CollectionOfSessions.Should().HaveCount(3);
                target.CollectionOfSessions.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
                target.CollectionOfSessions.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
                target.CollectionOfSessions.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
            }

            [Test]
            public void ShouldAdaptArrayOfSessions()
            {
                _source.ArrayOfSessions = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Session()}.ToList();
                var expected = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Session()};
                A.CallTo(() => _sessionAdapter.Adapt(_source.ArrayOfSessions.ElementAt(0))).Returns(expected.ElementAt(0));
                A.CallTo(() => _sessionAdapter.Adapt(_source.ArrayOfSessions.ElementAt(1))).Returns(expected.ElementAt(1));
                A.CallTo(() => _sessionAdapter.Adapt(_source.ArrayOfSessions.ElementAt(2))).Returns(expected.ElementAt(2));
                var target = _sut.Adapt(_source);
                target.ArrayOfSessions.Should().HaveCount(3);
                target.ArrayOfSessions.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
                target.ArrayOfSessions.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
                target.ArrayOfSessions.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
            }
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }
  }
}