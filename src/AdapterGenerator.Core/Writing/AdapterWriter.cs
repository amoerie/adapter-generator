using System;
using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.Adapters;

namespace AdapterGenerator.Core.Writing {
  public class AdapterWriter : IAdapterWriter {
    private readonly IFileWriter _fileWriter;
    private readonly ISyntaxFormatter _syntaxFormatter;

    public AdapterWriter(IFileWriter fileWriter, ISyntaxFormatter syntaxFormatter) {
      if (fileWriter == null) throw new ArgumentNullException(nameof(fileWriter));
      if (syntaxFormatter == null) throw new ArgumentNullException(nameof(syntaxFormatter));
      _fileWriter = fileWriter;
      _syntaxFormatter = syntaxFormatter;
    }

    public void Write(IImmutableList<IGeneratedAdapter> generatedAdapters, DirectoryInfo outputDirectory) {
      foreach (var adapter in generatedAdapters) {
        var path = Path.Combine(outputDirectory.FullName, $"{adapter.Blueprint.Name.Identifier.Text}.cs");
        var code = _syntaxFormatter.Format(adapter.CompilationUnitSyntax);
        _fileWriter.WriteFile(path, code);
      }
    }
  }
}