using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace AdapterGenerator.Tests.Utilities.CompareNetObjects {
  public partial class ExtensionsForT {
    public static IDictionary<T, IEnumerable<ComparisonResult>> CompareMany<T>(this IEnumerable<T> first, IEnumerable<T> second) {
      return CompareMany(first, second, config => {});
    }

    public static IDictionary<T, IEnumerable<ComparisonResult>> CompareMany<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<string> membersToIgnore) {
      return CompareMany(first, second, config => {
        config.MembersToIgnore = membersToIgnore?.ToList() ?? new List<string>();
      });
    }
    
    public static IDictionary<T, IEnumerable<ComparisonResult>> CompareMany<T>(this IEnumerable<T> first, IEnumerable<T> second, Action<ComparisonConfig> config) {
      if (first == null && second == null) return new Dictionary<T, IEnumerable<ComparisonResult>>();
      if (first == null || second == null) return new Dictionary<T, IEnumerable<ComparisonResult>>();

      var defaultConfig = new ComparisonConfig {
        IgnoreObjectTypes = true, // allows anonymous types to be compared
        IgnoreCollectionOrder = true
      };
      config?.Invoke(defaultConfig);
      var compareLogic = new CompareLogic(defaultConfig);
      
      var materializedFirst = first.ToList();
      var materializedSecond = second.ToList();
      var keyValuePairs = materializedFirst.Select(itemFromFirst => new KeyValuePair<T, IEnumerable<ComparisonResult>>(
        itemFromFirst, 
        materializedSecond.Select(itemFromSecond =>  compareLogic.Compare(itemFromFirst, itemFromSecond))));

      return keyValuePairs.ToDictionary(x => x.Key, x => x.Value);
    }
  }
}