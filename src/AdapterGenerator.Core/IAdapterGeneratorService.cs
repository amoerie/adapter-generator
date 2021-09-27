using System.Collections.Immutable;
using System.IO;

namespace AdapterGenerator.Core {
  public interface IAdapterGeneratorService {
    /// <summary>
    /// Generates an adapter and according unit tests for every pair of matching classes it can find in the source files and target files
    /// and writes this adapter and its unit tests to the specified output directory.
    /// </summary>
    /// <param name="sourceFiles">The source files.</param>
    /// <param name="targetFiles">The target files.</param>
    /// <param name="outputDirectory">The output directory.</param>
    void GenerateAdapters(IImmutableList<FileInfo> sourceFiles, IImmutableList<FileInfo> targetFiles,
      DirectoryInfo outputDirectory);
  }
}