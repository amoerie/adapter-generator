using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.AdapterTests;

namespace AdapterGenerator.Core.Writing {
  public interface IAdapterTestsWriter {
    void Write(IImmutableList<IGeneratedAdapterTests> generatedAdapterTests, DirectoryInfo outputDirectory);
  }
}