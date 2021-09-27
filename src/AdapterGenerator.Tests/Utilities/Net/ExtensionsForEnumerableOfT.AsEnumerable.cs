using System.Collections.Generic;
using System.Linq;

namespace AdapterGenerator.Tests.Utilities.Net {
  public static partial class ExtensionsForEnumerableOfT {
    public static IEnumerable<T> AsEnumerable<T>(this T instance) {
      return instance.IsNullOrDefault()
        ? Enumerable.Empty<T>()
        : new[] { instance };
    }
  }
}