using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using Autofac;
using NUnit.Framework;

namespace AdapterGenerator.Tests.IntegrationTests {
  [TestFixture]
  public class SoapReferenceAdapterTests {
    IAdapterGeneratorService _adapterGeneratorService;

    [SetUp]
    public virtual void SetUp() {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      _adapterGeneratorService = container.Resolve<IAdapterGeneratorService>();
    }

    [Test]
    public void ShouldGenerateCorrectAdapterForASoapReferenceClass() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceDepartment);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetDepartment);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "DepartmentAdapter.cs"));
      var expectedFileContents =
        @"using System;
using System.Collections.Generic;
using System.Linq;
using Broka.WebApi.Api.Models.Phones;

namespace Broka.WebApi.Api.Models.Departments
{
    public class DepartmentAdapter : IAdapter<Broka.WebApi.Data.GeneratedWebService.BrokaDepartment, Broka.WebApi.Api.Models.Departments.Department>
    {
        private readonly IAdapter<Broka.WebApi.Data.GeneratedWebService.BrokaPhoneNumber, Broka.WebApi.Api.Models.Phones.PhoneNumber> _phoneNumberAdapter;
        public DepartmentAdapter(IAdapter<Broka.WebApi.Data.GeneratedWebService.BrokaPhoneNumber, Broka.WebApi.Api.Models.Phones.PhoneNumber> phoneNumberAdapter)
        {
            if (phoneNumberAdapter == null)
                throw new ArgumentNullException(nameof(phoneNumberAdapter));
            _phoneNumberAdapter = phoneNumberAdapter;
        }

        public Broka.WebApi.Api.Models.Departments.Department Adapt(Broka.WebApi.Data.GeneratedWebService.BrokaDepartment source)
        {
            if (source == null)
                return null;
            var target = new Broka.WebApi.Api.Models.Departments.Department();
            target.Id = source.Id;
            target.UniqueIdentifier = source.UniqueIdentifier;
            target.Abbreviation = source.Abbreviation;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Phone = _phoneNumberAdapter.Adapt(source.Phone);
            target.ContactName = source.ContactName;
            target.ContactEmail = source.ContactMail;
            target.ContactPhone = _phoneNumberAdapter.Adapt(source.ContactPhone);
            target.ExternalCode = source.ExternalCode;
            target.FutureBookDateInDays = source.FutureBookDateInDays;
            return target;
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }

    [Test]
    public void ShouldGenerateCorrectAdapterTestsForASoapReferenceClass() {
      var sourceFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.SourceDepartment);
      var targetFiles = ImmutableList.Create(TestDataIndex.IntegrationTests.TargetDepartment);
      var outputDirectory = new DirectoryInfo(Path.GetTempPath());
      _adapterGeneratorService.GenerateAdapters(sourceFiles, targetFiles, outputDirectory);
      var file = new FileInfo(Path.Combine(outputDirectory.FullName, "DepartmentAdapterTests.cs"));
      var expectedFileContents =
        @"using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Broka.WebApi.Api.Models.Phones;

namespace Broka.WebApi.Api.Models.Departments
{
    [TestFixture]
    public class DepartmentAdapterTests
    {
        private Broka.WebApi.Api.Models.Departments.DepartmentAdapter _sut;
        private IAdapter<Broka.WebApi.Data.GeneratedWebService.BrokaPhoneNumber, Broka.WebApi.Api.Models.Phones.PhoneNumber> _phoneNumberAdapter;
        [SetUp]
        public virtual void SetUp()
        {
            _phoneNumberAdapter = _phoneNumberAdapter.Fake();
            _sut = new Broka.WebApi.Api.Models.Departments.DepartmentAdapter(_phoneNumberAdapter);
        }

        [TestFixture]
        public class Constructor : DepartmentAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : DepartmentAdapterTests
        {
            private Broka.WebApi.Data.GeneratedWebService.BrokaDepartment _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new Broka.WebApi.Data.GeneratedWebService.BrokaDepartment();
            }

            [Test]
            public void ShouldReturnNullWhenSourceIsNull()
            {
                _source = null;
                var target = _sut.Adapt(_source);
                target.Should().BeNull();
            }

            [Test]
            public void ShouldAdaptId()
            {
                _source.Id = 42;
                var target = _sut.Adapt(_source);
                target.Id.ShouldBeEquivalentTo(_source.Id);
            }

            [Test]
            public void ShouldAdaptUniqueIdentifier()
            {
                _source.UniqueIdentifier = default (System.Guid);
                var target = _sut.Adapt(_source);
                target.UniqueIdentifier.ShouldBeEquivalentTo(_source.UniqueIdentifier);
            }

            [Test]
            public void ShouldAdaptAbbreviation()
            {
                _source.Abbreviation = ""abc"";
                var target = _sut.Adapt(_source);
                target.Abbreviation.ShouldBeEquivalentTo(_source.Abbreviation);
            }

            [Test]
            public void ShouldAdaptName()
            {
                _source.Name = ""abc"";
                var target = _sut.Adapt(_source);
                target.Name.ShouldBeEquivalentTo(_source.Name);
            }

            [Test]
            public void ShouldAdaptDescription()
            {
                _source.Description = ""abc"";
                var target = _sut.Adapt(_source);
                target.Description.ShouldBeEquivalentTo(_source.Description);
            }

            [Test]
            public void ShouldAdaptPhone()
            {
                _source.Phone = new Broka.WebApi.Data.GeneratedWebService.BrokaPhoneNumber();
                var targetPhoneNumber = new Broka.WebApi.Api.Models.Phones.PhoneNumber();
                A.CallTo(() => _phoneNumberAdapter.Adapt(_source.Phone)).Returns(targetPhoneNumber);
                var target = _sut.Adapt(_source);
                target.Phone.Should().Be(targetPhoneNumber);
            }

            [Test]
            public void ShouldAdaptContactName()
            {
                _source.ContactName = ""abc"";
                var target = _sut.Adapt(_source);
                target.ContactName.ShouldBeEquivalentTo(_source.ContactName);
            }

            [Test]
            public void ShouldAdaptContactEmail()
            {
                _source.ContactMail = ""abc"";
                var target = _sut.Adapt(_source);
                target.ContactEmail.ShouldBeEquivalentTo(_source.ContactMail);
            }

            [Test]
            public void ShouldAdaptContactPhone()
            {
                _source.ContactPhone = new Broka.WebApi.Data.GeneratedWebService.BrokaPhoneNumber();
                var targetPhoneNumber = new Broka.WebApi.Api.Models.Phones.PhoneNumber();
                A.CallTo(() => _phoneNumberAdapter.Adapt(_source.ContactPhone)).Returns(targetPhoneNumber);
                var target = _sut.Adapt(_source);
                target.ContactPhone.Should().Be(targetPhoneNumber);
            }

            [Test]
            public void ShouldAdaptExternalCode()
            {
                _source.ExternalCode = ""abc"";
                var target = _sut.Adapt(_source);
                target.ExternalCode.ShouldBeEquivalentTo(_source.ExternalCode);
            }

            [Test]
            public void ShouldAdaptFutureBookDateInDays()
            {
                _source.FutureBookDateInDays = 42;
                var target = _sut.Adapt(_source);
                target.FutureBookDateInDays.ShouldBeEquivalentTo(_source.FutureBookDateInDays);
            }
        }
    }
}";
      var actualFileContents = File.ReadAllText(file.FullName);
      actualFileContents.ShouldBeSameCodeAs(expectedFileContents);
    }
  }
}