using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets
{
  public class Employee
  {
    public int Integer { get; set; }
    public double Double { get; set; }
    public string String { get; set; }
    public IEnumerable<int> Integers { get; set; }
    public ICollection<double> Doubles { get; set; }
    public string[] Strings { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? NullableDateTime { get; set; }
    public IImmutableList<IEnumerable<DateTime>> DateTimes { get; set; }
  }
}
