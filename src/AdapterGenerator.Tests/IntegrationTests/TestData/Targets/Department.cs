using System;
using Broka.WebApi.Api.Models.Phones;

namespace Broka.WebApi.Api.Models.Departments {
  public class Department {
    public int Id { get; set; }
    public Guid UniqueIdentifier { get; set; }
    public string Abbreviation { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PhoneNumber Phone { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public PhoneNumber ContactPhone { get; set; }
    public string ExternalCode { get; set; }
    public short FutureBookDateInDays { get; set; }
  }
}

namespace Broka.WebApi.Api.Models.Phones {
  public class PhoneNumber {
    public string Number { get; set; }
    public PhoneNumberType Type { get; set; }
    public PhoneNumberUsage Usage { get; set; }
  }

  public enum PhoneNumberType {
    Phone,
    Fax,
    Mobile,
    Other
  }

  public enum PhoneNumberUsage {
    Private,
    Business
  }
}