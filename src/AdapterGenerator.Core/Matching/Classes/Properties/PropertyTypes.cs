using System.Collections.Immutable;

namespace AdapterGenerator.Core.Matching.Classes.Properties {
  public class PropertyTypes {
    public const string IEnumerable = "IEnumerable";

    public static readonly IImmutableSet<string> KnownCollectionTypes = ImmutableHashSet.Create("ICollection",
      "Collection", "IList", "List");

    public static readonly IImmutableSet<string> KnownEnumerableTypes =
      ImmutableHashSet.Create(IEnumerable, "ISet").Union(KnownCollectionTypes);
  }
}