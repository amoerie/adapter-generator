namespace AdapterGenerator.Tests.Core.Analysis.TestData {
  public class FindPersonResponse {
    public Person Person { get; set; }
  }

  public class Person {
    public int Id { get; set; }
    public Address Address { get; set; }
  }

  public class Address {
    public int Id { get; set; }
  }

  public enum Gender {
    Male,
    Female
  }
}