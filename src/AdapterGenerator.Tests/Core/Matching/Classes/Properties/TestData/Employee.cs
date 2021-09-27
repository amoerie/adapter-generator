using System;
using System.Collections.Generic;

namespace AdapterGenerator.Tests.Core.Matching.Properties.TestData {
  public class Employee {
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
    public IEnumerable<string> Strings { get; set; }
    public string[] StringsArray { get; set; }

    public IEnumerable<Address> Addresses { get; set; }
    public IEnumerable<Address> WorkAddresses { get; set; }
    public Address[] DeliveryAddresses { get; set; }
    public Address[] SecondaryAddresses { get; set; }

    public DateTime? BirthDate { get; set; }

    public IEnumerable<int> Ints { get; set; }
    public Gender Gender { get; set; }
  }

  public class Address {
    public string Street { get; set; }
  }

  public enum Gender {
    Male,
    Female
  }
}