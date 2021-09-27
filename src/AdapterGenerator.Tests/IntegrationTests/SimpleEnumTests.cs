using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using Autofac;
using NUnit.Framework;

namespace AdapterGenerator.Tests.IntegrationTests {
  [TestFixture]
  public class SimpleEnumAdapterTests {
    IAdapterGeneratorService _adapterGeneratorService;

    [SetUp]
    public virtual void SetUp() {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      _adapterGeneratorService = container.Resolve<IAdapterGeneratorService>();
    }

    [Test]
    public void ShouldGenerateAdapterForASimpleEnumWithSomeValues() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceGender);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetGender);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var genderAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "GenderAdapter.cs"));
      var expectedFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class GenderAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender>
    {
        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender source)
        {
            switch (source)
            {
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender.Male:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender.Male;
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender.Female:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender.Female;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, $""Cannot adapt {source} to a matching value"");
            }
        }
    }
}";
      var actualFileContents = File.ReadAllText(genderAdapterFile.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateAdapterTestsForASimpleEnumWithSomeValues() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceGender);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetGender);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "GenderAdapterTests.cs"));
      var fileContents =
        @"using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    [TestFixture]
    public class GenderAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.GenderAdapter _sut;
        [SetUp]
        public virtual void SetUp()
        {
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.GenderAdapter();
        }

        [TestFixture]
        public class Constructor : GenderAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : GenderAdapterTests
        {
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender.Male, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender.Male)]
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender.Female, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender.Female)]
            public void ShouldAdaptCorrectly(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Gender source, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Gender target)
            {
                _sut.Adapt(source).Should().Be(target);
            }
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(fileContents);
    }
  }
}