using System;
using System.Collections.Generic;

namespace AdapterGenerator.Tests.Core.Generation.Adapters.TestData {
  public class Source {
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public DateTime? NullableDate { get; set; }
    public IEnumerable<int> Numbers { get; }
    public Nested Nested { get; set; }
    public IEnumerable<Nested> Nesteds { get; set; }
  }
}