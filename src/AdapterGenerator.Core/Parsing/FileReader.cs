using System;
using System.IO;
using AdapterGenerator.Core.Logging;

namespace AdapterGenerator.Core.Parsing {
  public class FileReader : IFileReader {
    private readonly ILogger _logger;

    public FileReader(ILogger logger) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      _logger = logger;
    }

    public string ReadFile(string absolutePath) {
      _logger.Info($"Reading {absolutePath}");
      return File.ReadAllText(absolutePath);
    }
  }
}