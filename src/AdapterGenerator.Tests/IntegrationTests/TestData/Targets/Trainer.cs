using System.Collections.Generic;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets {
  public class Trainer {
    public string Name { get; set; }
    public IEnumerable<Pokémon> CaughtPokémon { get; set; }
    public IEnumerable<Pokémon> SeenPokémon { get; set; }

    public class Pokémon {
      public string Name { get; set; }
      public PokémonType Type { get; set; }

      public enum PokémonType {
        Water,
        Fire,
        Grass,
        Psychic,
        Ghost
      }
    }
  }
}