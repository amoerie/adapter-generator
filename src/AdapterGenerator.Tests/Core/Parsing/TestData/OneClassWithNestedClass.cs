using System;

namespace AdapterGenerator.Tests.Core.Analysis.TestData {
  public class Person {
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public Address PersonAddress { get; set; }

    public class Address {
      public string Street { get; set; }
      public string City { get; set; }
      public string Number { get; set; }
    }
  }
}