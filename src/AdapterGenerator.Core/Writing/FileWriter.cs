using System;
using System.IO;
using AdapterGenerator.Core.Logging;

namespace AdapterGenerator.Core.Writing {
  public class FileWriter : IFileWriter {
    private readonly ILogger _logger;

    public FileWriter(ILogger logger) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      _logger = logger;
    }

    public void WriteFile(string path, string contents) {
      _logger.Info($"Writing content to {path}");
      File.WriteAllText(path, contents);
    }
  }
}