using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;

namespace AdapterGenerator.Tests.Utilities.FluentAssertions {
  public static class ExtensionsForTypeAssertions {
    public static AndConstraint<TypeAssertions> HaveExactlyOneConstructorWithoutOptionalParameters(this TypeAssertions typeAssertions) {
      return HaveExactlyOneConstructorWithoutOptionalParameters(typeAssertions, false);
    }

    public static AndConstraint<TypeAssertions> HaveExactlyOneConstructorWithoutOptionalParameters(this TypeAssertions typeAssertions, bool includeNonPublicConstructors) {
      var bindingFlags = includeNonPublicConstructors
        ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        : BindingFlags.Public | BindingFlags.Instance;
      var publicConstructors = typeAssertions.Subject.GetConstructors(bindingFlags);
      Execute.Assertion.ForCondition(publicConstructors.Length == 1)
        .BecauseOf("because there should only be 1 public constructor")
        .FailWith("Expected {0} to have 1 public constructor{reason} but found {1} public constructors: {2}", typeAssertions.Subject, publicConstructors.Length,
          string.Join(", ", publicConstructors.Select((c, i) =>
            $"constructor {i}: ({string.Join(", ", c.GetParameters().Select(p => p.Name))})")));
      publicConstructors.Single().Should().NotHaveOptionalParameters();
      return new AndConstraint<TypeAssertions>(typeAssertions);
    }
  }
}