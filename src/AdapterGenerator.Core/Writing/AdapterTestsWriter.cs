using System;
using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.AdapterTests;

namespace AdapterGenerator.Core.Writing {
  public class AdapterTestsWriter : IAdapterTestsWriter {
    private readonly IFileWriter _fileWriter;
    private readonly ISyntaxFormatter _syntaxFormatter;

    public AdapterTestsWriter(IFileWriter fileWriter, ISyntaxFormatter syntaxFormatter) {
      if (fileWriter == null) throw new ArgumentNullException(nameof(fileWriter));
      if (syntaxFormatter == null) throw new ArgumentNullException(nameof(syntaxFormatter));
      _fileWriter = fileWriter;
      _syntaxFormatter = syntaxFormatter;
    }

    public void Write(IImmutableList<IGeneratedAdapterTests> generatedAdapterTests, DirectoryInfo outputDirectory) {
      foreach (var adapterTest in generatedAdapterTests) {
        var path = Path.Combine(outputDirectory.FullName, $"{adapterTest.TestClassName}.cs");
        var code = _syntaxFormatter.Format(adapterTest.CompilationUnitSyntax);
        _fileWriter.WriteFile(path, code);
      }
    }
  }
}