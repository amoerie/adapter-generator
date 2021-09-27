using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using Autofac;
using NUnit.Framework;

namespace AdapterGenerator.Tests.IntegrationTests {
  [TestFixture]
  public class NestedAdapterTests {
    IAdapterGeneratorService _adapterGeneratorService;

    [SetUp]
    public virtual void SetUp() {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      _adapterGeneratorService = container.Resolve<IAdapterGeneratorService>();
    }

    [Test]
    public void ShouldGenerateCorrectAdapterForASimpleClassWithOnePredefinedProperty() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceCompany);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetCompany);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var companyAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "CompanyAdapter.cs"));
      var expectedCompanyAdapterFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class CompanyAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Company, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Company>
    {
        private readonly IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Address, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Address> _addressAdapter;
        public CompanyAdapter(IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Address, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Address> addressAdapter)
        {
            if (addressAdapter == null)
                throw new ArgumentNullException(nameof(addressAdapter));
            _addressAdapter = addressAdapter;
        }

        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Company Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Company source)
        {
            if (source == null)
                return null;
            var target = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Company();
            target.Address = _addressAdapter.Adapt(source.Address);
            return target;
        }
    }
}";
      var actualFileContents = File.ReadAllText(companyAdapterFile.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedCompanyAdapterFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterTestsForASimpleClassWithOnePredefinedProperty() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceCompany);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetCompany);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "CompanyAdapterTests.cs"));
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
    public class CompanyAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.CompanyAdapter _sut;
        private IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Address, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Address> _addressAdapter;
        [SetUp]
        public virtual void SetUp()
        {
            _addressAdapter = _addressAdapter.Fake();
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.CompanyAdapter(_addressAdapter);
        }

        [TestFixture]
        public class Constructor : CompanyAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : CompanyAdapterTests
        {
            private AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Company _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Company();
            }

            [Test]
            public void ShouldReturnNullWhenSourceIsNull()
            {
                _source = null;
                var target = _sut.Adapt(_source);
                target.Should().BeNull();
            }

            [Test]
            public void ShouldAdaptAddress()
            {
                _source.Address = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Address();
                var targetAddress = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Address();
                A.CallTo(() => _addressAdapter.Adapt(_source.Address)).Returns(targetAddress);
                var target = _sut.Adapt(_source);
                target.Address.Should().Be(targetAddress);
            }
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterForAClassWithDeeplyNestedSubclasses() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceTrainer);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetTrainer);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var trainerAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "TrainerAdapter.cs"));
      var expectedTrainerAdapterFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class TrainerAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer>
    {
        private readonly IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon> _pokémonAdapter;
        public TrainerAdapter(IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon> pokémonAdapter)
        {
            if (pokémonAdapter == null)
                throw new ArgumentNullException(nameof(pokémonAdapter));
            _pokémonAdapter = pokémonAdapter;
        }

        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer source)
        {
            if (source == null)
                return null;
            var target = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer();
            target.Name = source.Name;
            target.CaughtPokémon = source.CaughtPokémon?.Select(_pokémonAdapter.Adapt);
            target.SeenPokémon = source.SeenPokémon?.Select(_pokémonAdapter.Adapt);
            return target;
        }
    }
}";
      var actualTrainerAdapterFileContents = File.ReadAllText(trainerAdapterFile.FullName);
      actualTrainerAdapterFileContents.ShouldBeSameCodeAs(expectedTrainerAdapterFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterTestsForAClassWithDeeplyNestedSubclasses() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceTrainer);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetTrainer);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "TrainerAdapterTests.cs"));
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
    public class TrainerAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.TrainerAdapter _sut;
        private IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon> _pokémonAdapter;
        [SetUp]
        public virtual void SetUp()
        {
            _pokémonAdapter = _pokémonAdapter.Fake();
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.TrainerAdapter(_pokémonAdapter);
        }

        [TestFixture]
        public class Constructor : TrainerAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : TrainerAdapterTests
        {
            private AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer();
            }

            [Test]
            public void ShouldReturnNullWhenSourceIsNull()
            {
                _source = null;
                var target = _sut.Adapt(_source);
                target.Should().BeNull();
            }

            [Test]
            public void ShouldAdaptName()
            {
                _source.Name = ""abc"";
                var target = _sut.Adapt(_source);
                target.Name.ShouldBeEquivalentTo(_source.Name);
            }

            [Test]
            public void ShouldAdaptCaughtPokémon()
            {
                _source.CaughtPokémon = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon()};
                var expected = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon()};
                A.CallTo(() => _pokémonAdapter.Adapt(_source.CaughtPokémon.ElementAt(0))).Returns(expected.ElementAt(0));
                A.CallTo(() => _pokémonAdapter.Adapt(_source.CaughtPokémon.ElementAt(1))).Returns(expected.ElementAt(1));
                A.CallTo(() => _pokémonAdapter.Adapt(_source.CaughtPokémon.ElementAt(2))).Returns(expected.ElementAt(2));
                var target = _sut.Adapt(_source);
                target.CaughtPokémon.Should().HaveCount(3);
                target.CaughtPokémon.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
                target.CaughtPokémon.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
                target.CaughtPokémon.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
            }

            [Test]
            public void ShouldAdaptSeenPokémon()
            {
                _source.SeenPokémon = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon()};
                var expected = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon[]{new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon(), new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon()};
                A.CallTo(() => _pokémonAdapter.Adapt(_source.SeenPokémon.ElementAt(0))).Returns(expected.ElementAt(0));
                A.CallTo(() => _pokémonAdapter.Adapt(_source.SeenPokémon.ElementAt(1))).Returns(expected.ElementAt(1));
                A.CallTo(() => _pokémonAdapter.Adapt(_source.SeenPokémon.ElementAt(2))).Returns(expected.ElementAt(2));
                var target = _sut.Adapt(_source);
                target.SeenPokémon.Should().HaveCount(3);
                target.SeenPokémon.ElementAt(0).Should().BeSameAs(expected.ElementAt(0));
                target.SeenPokémon.ElementAt(1).Should().BeSameAs(expected.ElementAt(1));
                target.SeenPokémon.ElementAt(2).Should().BeSameAs(expected.ElementAt(2));
            }
        }
    }
}";
      var actualTrainerAdapterFileContents = File.ReadAllText(file.FullName);
      actualTrainerAdapterFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterForANestedSubclass() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceTrainer);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetTrainer);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var pokémonAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "PokémonAdapter.cs"));
      var expectedPokémonAdapterFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class PokémonAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon>
    {
        private readonly IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType> _pokémonTypeAdapter;
        public PokémonAdapter(IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType> pokémonTypeAdapter)
        {
            if (pokémonTypeAdapter == null)
                throw new ArgumentNullException(nameof(pokémonTypeAdapter));
            _pokémonTypeAdapter = pokémonTypeAdapter;
        }

        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon source)
        {
            if (source == null)
                return null;
            var target = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon();
            target.Name = source.Name;
            target.Type = _pokémonTypeAdapter.Adapt(source.Type);
            return target;
        }
    }
}";
      var actualPokémonAdapterFileContents = File.ReadAllText(pokémonAdapterFile.FullName);
      actualPokémonAdapterFileContents.ShouldBeSameCodeAs(expectedPokémonAdapterFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterForANestedEnum() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceTrainer);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetTrainer);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var pokémonTypeAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "PokémonTypeAdapter.cs"));
      var expectedPokémonTypeAdapterFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class PokémonTypeAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType>
    {
        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType source)
        {
            switch (source)
            {
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Water:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Water;
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Fire:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Fire;
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Grass:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Grass;
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Psychic:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Psychic;
                case AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Ghost:
                    return AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Ghost;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, $""Cannot adapt {source} to a matching value"");
            }
        }
    }
}";
      var actualPokémonTypeAdapterFileContents = File.ReadAllText(pokémonTypeAdapterFile.FullName);
      actualPokémonTypeAdapterFileContents.ShouldBeSameCodeAs(expectedPokémonTypeAdapterFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterTestsForANestedEnum() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceTrainer);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetTrainer);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "PokémonTypeAdapterTests.cs"));
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
    public class PokémonTypeAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.PokémonTypeAdapter _sut;
        [SetUp]
        public virtual void SetUp()
        {
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.PokémonTypeAdapter();
        }

        [TestFixture]
        public class Constructor : PokémonTypeAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : PokémonTypeAdapterTests
        {
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Water, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Water)]
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Fire, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Fire)]
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Grass, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Grass)]
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Psychic, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Psychic)]
            [TestCase(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType.Ghost, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType.Ghost)]
            public void ShouldAdaptCorrectly(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Trainer.Pokémon.PokémonType source, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Trainer.Pokémon.PokémonType target)
            {
                _sut.Adapt(source).Should().Be(target);
            }
        }
    }
}";
      var actualPokémonTypeAdapterFileContents = File.ReadAllText(file.FullName);
      actualPokémonTypeAdapterFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }
  }
}