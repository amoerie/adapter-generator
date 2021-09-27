# README #

### What is this repository for? ###

* Automatically generate adapters & corresponding unit tests for C# POCO's

### How do I get set up? ###

* Download the latest version from the Downloads tab or clone the repository and build it yourself!

### How does this work? ###

You will need three things:

1. A number of "source" files to map from
2. A number of "target" files to map to
3. A directory to store the output in

Example:

1. SourceFile.cs
```csharp
namespace Example.Sources {
  public class Company {
    public string Name { get; set; }
    public Address Address { get; set; }
  }

  public class Address {
    public string City { get; set; }
  }
}
```

2. TargetFile.cs
```csharp
namespace Example.Targets {
  public class Company {
    public string Name { get; set; }
    public Address Address { get; set; }
  }

  public class Address {
    public string City { get; set; }
  }
}
```

3. Run the adapter generator: 
`AdapterGenerator.Cli.exe --source-files Example.Sources.cs --target-files Example.Targets.cs --output-directory ./Output`

Behold the output:

AddressAdapter.cs
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Targets
{
    public class AddressAdapter : IAdapter<Example.Sources.Address, Example.Targets.Address>
    {
        public AddressAdapter()
        {
        }

        public Example.Targets.Address Adapt(Example.Sources.Address source)
        {
            if (source == null)
                return null;
            var target = new Example.Targets.Address();
            target.City = source.City;
            return target;
        }
    }
}
```

AddressAdapterTests.cs
```csharp
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Targets
{
    [TestFixture]
    public class AddressAdapterTests
    {
        private Example.Targets.AddressAdapter _sut;
        [SetUp]
        public virtual void SetUp()
        {
            _sut = new Example.Targets.AddressAdapter();
        }

        [TestFixture]
        public class Constructor : AddressAdapterTests
        {
            [Test]
            public void ShouldHaveNoOptionalDependencies()
            {
                _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
            }
        }

        [TestFixture]
        public class Adapt : AddressAdapterTests
        {
            private Example.Sources.Address _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new Example.Sources.Address();
            }

            [Test]
            public void ShouldReturnNullWhenSourceIsNull()
            {
                _source = null;
                var target = _sut.Adapt(_source);
                target.Should().BeNull();
            }

            [Test]
            public void ShouldAdaptCity()
            {
                _source.City = "abc";
                var target = _sut.Adapt(_source);
                target.City.ShouldBeEquivalentTo(_source.City);
            }
        }
    }
}
```

CompanyAdapter.cs
```csharp

using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Targets
{
    public class CompanyAdapter : IAdapter<Example.Sources.Company, Example.Targets.Company>
    {
        private readonly IAdapter<Example.Sources.Address, Example.Targets.Address> _addressAdapter;
        public CompanyAdapter(IAdapter<Example.Sources.Address, Example.Targets.Address> addressAdapter)
        {
            if (addressAdapter == null)
                throw new ArgumentNullException(nameof(addressAdapter));
            _addressAdapter = addressAdapter;
        }

        public Example.Targets.Company Adapt(Example.Sources.Company source)
        {
            if (source == null)
                return null;
            var target = new Example.Targets.Company();
            target.Name = source.Name;
            target.Address = _addressAdapter.Adapt(source.Address);
            return target;
        }
    }
}
```

CompanyAdapterTests
```csharp
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Targets
{
    [TestFixture]
    public class CompanyAdapterTests
    {
        private Example.Targets.CompanyAdapter _sut;
        private IAdapter<Example.Sources.Address, Example.Targets.Address> _addressAdapter;
        
        [SetUp]
        public virtual void SetUp()
        {
            _addressAdapter = _addressAdapter.Fake();
            _sut = new Example.Targets.CompanyAdapter(_addressAdapter);
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
            private Example.Sources.Company _source;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _source = new Example.Sources.Company();
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
                _source.Name = "abc";
                var target = _sut.Adapt(_source);
                target.Name.ShouldBeEquivalentTo(_source.Name);
            }

            [Test]
            public void ShouldAdaptAddress()
            {
                _source.Address = new Example.Sources.Address();
                var targetAddress = new Example.Targets.Address();
                A.CallTo(() => _addressAdapter.Adapt(_source.Address)).Returns(targetAddress);
                var target = _sut.Adapt(_source);
                target.Address.Should().Be(targetAddress);
            }
        }
    }
}
```
