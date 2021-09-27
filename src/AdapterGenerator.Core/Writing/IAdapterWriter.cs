using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.Adapters;

namespace AdapterGenerator.Core.Writing {
  public interface IAdapterWriter {
    void Write(IImmutableList<IGeneratedAdapter> generatedAdapters, DirectoryInfo outputDirectory);
  }
}