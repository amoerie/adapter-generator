using System;
using System.Collections.Generic;
using AdapterGenerator.Tests.Utilities.CompareNetObjects;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;

namespace AdapterGenerator.Tests.Utilities.FakeItEasy {
  public static partial class ExtensionsForArgumentConstraintManagerOfT {
    public static IEnumerable<T> IsSameCollectionAs<T>(this IArgumentConstraintManager<IEnumerable<T>> manager, IEnumerable<T> value) {
      return IsSameCollectionAs(manager, value, config => {});
    }

    public static IEnumerable<T> IsSameCollectionAs<T>(this IArgumentConstraintManager<IEnumerable<T>> manager, IEnumerable<T> value, IEnumerable<string> membersToIgnore) {
      return manager.Matches(
        x => x.IsSameCollectionAs(value, membersToIgnore),
        x => x.Write("object that is same collection by property values as ").WriteArgumentValue(value));
    }

    public static IEnumerable<T> IsSameCollectionAs<T>(this IArgumentConstraintManager<IEnumerable<T>> manager, IEnumerable<T> value, Action<ComparisonConfig> config) {
      return manager.Matches(
        x => x.IsSameCollectionAs(value, config),
        x => x.Write("object that is same collection by property values as ").WriteArgumentValue(value));
    }
  }
}