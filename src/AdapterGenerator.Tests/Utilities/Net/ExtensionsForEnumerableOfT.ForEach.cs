using System;
using System.Collections.Generic;

namespace AdapterGenerator.Tests.Utilities.Net {
  public static partial class ExtensionsForEnumerableOfT {
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
      foreach (var element in source) {
        action(element);
      }
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action) {
      var index = 0;
      foreach (var element in source) {
        action(element, index++);
      }
    }
  }
}