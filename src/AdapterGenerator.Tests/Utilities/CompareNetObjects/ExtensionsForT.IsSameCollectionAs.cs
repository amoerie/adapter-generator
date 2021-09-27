using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace AdapterGenerator.Tests.Utilities.CompareNetObjects {
  public partial class ExtensionsForT {
    public static bool IsSameCollectionAs<T>(this IEnumerable<T> first, IEnumerable<T> second) {
      return IsSameCollectionAs(first, second, config => {});
    }

    public static bool IsSameCollectionAs<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<string> membersToIgnore) {
      return IsSameCollectionAs(first, second, config => {
        config.MembersToIgnore = membersToIgnore?.ToList() ?? new List<string>();
      });
    }

    public static bool IsSameCollectionAs<T>(this IEnumerable<T> first, IEnumerable<T> second, Action<ComparisonConfig> config) {
      if (first == null && second == null) return true;
      if (first == null || second == null) return false;

      var defaultConfig = new ComparisonConfig {
        IgnoreObjectTypes = true, // allows anonymous types to be compared
        IgnoreCollectionOrder = true
      };
      config?.Invoke(defaultConfig);
      var compareLogic = new CompareLogic(defaultConfig);

      var materializedFirst = first.ToList();
      var materializedSecond = second.ToList();
      return
        materializedFirst.Count == materializedSecond.Count &&
        materializedFirst.All(
          itemFromFirst =>
            materializedSecond.Any(itemFromSecond => compareLogic.Compare(itemFromFirst, itemFromSecond).AreEqual));
    }
  }
}