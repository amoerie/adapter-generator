using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using Autofac;
using NUnit.Framework;

namespace AdapterGenerator.Tests.IntegrationTests {
  [TestFixture]
  public class SimpleClassAdapterTests {
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
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourcePerson);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetPerson);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "PersonAdapter.cs"));
      var expectedFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class PersonAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Person, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Person>
    {
        public PersonAdapter()
        {
        }

        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Person Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Person source)
        {
            if (source == null)
                return null;
            var target = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Person();
            target.Name = source.Name;
            return target;
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateAdapterTestsForASimpleClassWithOnePredefinedProperty() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourcePerson);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetPerson);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "PersonAdapterTests.cs"));
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
    public class PersonAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.PersonAdapter _sut;
        [SetUp]
        public virtual void SetUp()
        {
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.PersonAdapter();
        }

        [TestFixture]
        public class Constructor : PersonAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : PersonAdapterTests
        {
            private AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Person _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Person();
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
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateAdapterForASimpleClassWithoutNestedClasses() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceEmployee);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetEmployee);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var personAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "EmployeeAdapter.cs"));
      var expectedPersonAdapterFileContents = @"using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    public class EmployeeAdapter : IAdapter<AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Employee, AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Employee>
    {
        public EmployeeAdapter()
        {
        }

        public AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Employee Adapt(AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Employee source)
        {
            if (source == null)
                return null;
            var target = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.Employee();
            target.Integer = source.Integer;
            target.Double = source.Double;
            target.String = source.String;
            target.Integers = source.Integers;
            target.Doubles = source.Doubles;
            target.Strings = source.Strings;
            target.DateTime = source.DateTime;
            target.NullableDateTime = source.NullableDateTime;
            target.DateTimes = source.DateTimes;
            return target;
        }
    }
}";
      var actualFileContents = File.ReadAllText(personAdapterFile.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedPersonAdapterFileContents);
    }

    [Test]
    public void ShouldGenerateAdapterTestsForASimpleClassWithoutNestedClasses() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceEmployee);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetEmployee);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var personAdapterFile = new FileInfo(Path.Combine(outputDirectory.FullName, "EmployeeAdapterTests.cs"));
      var expectedPersonAdapterFileContents = @"using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
    [TestFixture]
    public class EmployeeAdapterTests
    {
        private AdapterGenerator.Tests.IntegrationTests.TestData.Targets.EmployeeAdapter _sut;
        [SetUp]
        public virtual void SetUp()
        {
            _sut = new AdapterGenerator.Tests.IntegrationTests.TestData.Targets.EmployeeAdapter();
        }

        [TestFixture]
        public class Constructor : EmployeeAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : EmployeeAdapterTests
        {
            private AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Employee _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new AdapterGenerator.Tests.IntegrationTests.TestData.Sources.Employee();
            }

            [Test]
            public void ShouldReturnNullWhenSourceIsNull()
            {
                _source = null;
                var target = _sut.Adapt(_source);
                target.Should().BeNull();
            }

            [Test]
            public void ShouldAdaptInteger()
            {
                _source.Integer = 42;
                var target = _sut.Adapt(_source);
                target.Integer.ShouldBeEquivalentTo(_source.Integer);
            }

            [Test]
            public void ShouldAdaptDouble()
            {
                _source.Double = 42;
                var target = _sut.Adapt(_source);
                target.Double.ShouldBeEquivalentTo(_source.Double);
            }

            [Test]
            public void ShouldAdaptString()
            {
                _source.String = ""abc"";
                var target = _sut.Adapt(_source);
                target.String.ShouldBeEquivalentTo(_source.String);
            }

            [Test]
            public void ShouldAdaptIntegers()
            {
                _source.Integers = new int[]{42, 42, 42};
                var target = _sut.Adapt(_source);
                target.Integers.ShouldBeEquivalentTo(_source.Integers);
            }

            [Test]
            public void ShouldAdaptDoubles()
            {
                _source.Doubles = new double[]{42, 42, 42}.ToList();
                var target = _sut.Adapt(_source);
                target.Doubles.ShouldBeEquivalentTo(_source.Doubles);
            }

            [Test]
            public void ShouldAdaptStrings()
            {
                _source.Strings = new string[]{""abc"", ""abc"", ""abc""};
                var target = _sut.Adapt(_source);
                target.Strings.ShouldBeEquivalentTo(_source.Strings);
            }

            [Test]
            public void ShouldAdaptDateTime()
            {
                _source.DateTime = new DateTime(1990, 6, 2);
                var target = _sut.Adapt(_source);
                target.DateTime.ShouldBeEquivalentTo(_source.DateTime);
            }

            [Test]
            public void ShouldAdaptNullableDateTime()
            {
                _source.NullableDateTime = new DateTime(1990, 6, 2);
                var target = _sut.Adapt(_source);
                target.NullableDateTime.ShouldBeEquivalentTo(_source.NullableDateTime);
            }

            [Test]
            public void ShouldAdaptDateTimes()
            {
                _source.DateTimes = default (IImmutableList<IEnumerable<DateTime>>);
                var target = _sut.Adapt(_source);
                target.DateTimes.ShouldBeEquivalentTo(_source.DateTimes);
            }
        }
    }
}";
      var actualFileContents = File.ReadAllText(personAdapterFile.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedPersonAdapterFileContents);
    }
  }
}