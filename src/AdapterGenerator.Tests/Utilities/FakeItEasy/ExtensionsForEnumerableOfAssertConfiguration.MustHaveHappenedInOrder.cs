using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.Configuration;

namespace AdapterGenerator.Tests.Utilities.FakeItEasy {
  public static partial class ExtensionsForEnumerableOfAssertConfiguration {
    [Obsolete("Please use the documented Ordered Assertions for FakeItEasy (see https://fakeiteasy.readthedocs.io/en/stable/ordered-assertions/).")]
    public static IOrderableCallAssertion MustHaveHappenedInOrder(this IEnumerable<IAssertConfiguration> calls) {
      if (calls == null) throw new ArgumentNullException(nameof(calls));
      UnorderedCallAssertion orderedChain = null;
      foreach (var call in calls) {
        if (orderedChain == null) {
          orderedChain = call.MustHaveHappened();
        }
        else {
          orderedChain.Then(call.MustHaveHappened());
        }
      }
      if (orderedChain == null) throw new ArgumentException("No calls were specified.", nameof(calls));
      return orderedChain;
    }
  }
}