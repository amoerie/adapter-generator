using System;
using System.Collections.Generic;

namespace AdapterGenerator.Tests.Core.Matching.Properties.TestData {
  public class Manager {
    public string Name { get; set; }
    public string Age { get; set; }
    public bool HasCompanyCar { get; set; }
    public Address Address { get; set; }

    public IEnumerable<string> Strings { get; set; }
    public string[] StringsArray { get; set; }

    public IEnumerable<Address> Addresses { get; set; }
    public Address[] WorkAddresses { get; set; }
    public IEnumerable<Address> DeliveryAddresses { get; set; }
    public Address[] SecondaryAddresses { get; set; }

    public DateTime BirthDate { get; set; }

    public int?[] Ints { get; set; }
    public Gender Gender { get; set; }
  }

  public class Address {
    public string Street { get; set; }
  }

  public enum Gender
  {
    Male,
    Female
  }
}