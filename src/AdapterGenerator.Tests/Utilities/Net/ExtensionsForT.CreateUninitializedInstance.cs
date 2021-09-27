using System.Runtime.Serialization;

namespace AdapterGenerator.Tests.Utilities.Net {
  public static partial class ExtensionsForT {
    public static T CreateUninitializedInstance<T>(this T reference) where T : class {
      return (T)FormatterServices.GetUninitializedObject(typeof(T));
    }
  }
}