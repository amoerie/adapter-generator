using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;

namespace AdapterGenerator.Tests.Utilities.Net {
  public static partial class ExtensionsForT {
    public static bool NoDependenciesAreOptional<T>(this T reference, bool includeNonPublicConstructors) {
      var bindingFlags = includeNonPublicConstructors
        ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        : BindingFlags.Public | BindingFlags.Instance;

      var constructors = typeof(T).GetConstructors(bindingFlags);
      if (constructors.Length < 1) {
        throw new NoPublicConstructorsException();
      }
      if (constructors.Length > 1) {
        throw new MultiplePublicConstructorsException();
      }

      var ctor = constructors.Single();
      var dummyMethod = typeof(A).GetMethod("Dummy");
      var ctorArguments = ctor.GetParameters().Select(p => dummyMethod.MakeGenericMethod(p.ParameterType).Invoke(null, null)).ToArray();

      if (ctorArguments.Length < 1) {
        return true;
      }
      for (var i = 0; i < ctorArguments.Length; i++) {
        try {
          var args = new object[ctorArguments.Length];
          Array.Copy(ctorArguments, args, ctorArguments.Length);
          args[i] = null;
          ctor.Invoke(args);
          return false;
        } catch (TargetInvocationException ex) {
          if (!(ex.InnerException is ArgumentNullException)) {
            // Not an ArgumentNullException
            return false;
          }
        } catch (ArgumentNullException) {
          // Ctor fails with ArgumentNullException when null is passed, as expected
        }
      }
      return true;
    }

    public static bool NoDependenciesAreOptional<T>(this T reference) {
      return NoDependenciesAreOptional(reference, false);
    }

    public class MultiplePublicConstructorsException : Exception {}

    public class NoPublicConstructorsException : Exception {}
  }
}