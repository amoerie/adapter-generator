using System;
using System.Collections.Immutable;
using System.IO;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Core.Writing;

namespace AdapterGenerator.Core {
  public class AdapterGeneratorService : IAdapterGeneratorService {
    private readonly ILogger _logger;
    private readonly IFileParser _fileParser;
    private readonly IMatcher _matcher;
    private readonly IAdaptersGenerator _adaptersGenerator;
    private readonly IAdapterWriter _adapterWriter;
    private readonly IAdapterTestsGenerator _adapterTestsGenerator;
    private readonly IAdapterTestsWriter _adapterTestsWriter;

    public AdapterGeneratorService(ILogger logger, IFileParser fileParser, IMatcher matcher,
      IAdaptersGenerator adaptersGenerator, IAdapterWriter adapterWriter,
      IAdapterTestsGenerator adapterTestsGenerator, IAdapterTestsWriter adapterTestsWriter) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      if (fileParser == null) throw new ArgumentNullException(nameof(fileParser));
      if (matcher == null) throw new ArgumentNullException(nameof(matcher));
      if (adaptersGenerator == null) throw new ArgumentNullException(nameof(adaptersGenerator));
      if (adapterWriter == null) throw new ArgumentNullException(nameof(adapterWriter));
      if (adapterTestsGenerator == null) throw new ArgumentNullException(nameof(adapterTestsGenerator));
      if (adapterTestsWriter == null) throw new ArgumentNullException(nameof(adapterTestsWriter));
      _logger = logger;
      _fileParser = fileParser;
      _matcher = matcher;
      _adaptersGenerator = adaptersGenerator;
      _adapterWriter = adapterWriter;
      _adapterTestsGenerator = adapterTestsGenerator;
      _adapterTestsWriter = adapterTestsWriter;
    }

    public void GenerateAdapters(IImmutableList<FileInfo> sourceFiles, IImmutableList<FileInfo> targetFiles,
      DirectoryInfo outputDirectory) {
      if (sourceFiles == null) throw new ArgumentNullException(nameof(sourceFiles));
      if (targetFiles == null) throw new ArgumentNullException(nameof(targetFiles));
      if (outputDirectory == null) throw new ArgumentNullException(nameof(outputDirectory));
      _logger.Info("### PHASE 1 : Parsing");
      _logger.Info($"Parsing {sourceFiles.Count} source files and {targetFiles.Count} target files");
      var sources = _fileParser.ParseFiles(sourceFiles);
      var targets = _fileParser.ParseFiles(targetFiles);
      _logger.Info("### PHASE 2 : Matching");
      _logger.Info($"Matching {sources.Classes.Count} source classes with {targets.Classes.Count} target classes");
      _logger.Info($"Matching {sources.Enums.Count} source enums with {targets.Enums.Count} target enums");
      var matches = _matcher.Match(sources, targets);
      _logger.Info($"Found {matches.ClassMatches.Count} matching classes.");
      _logger.Info($"Found {matches.EnumMatches.Count} matching enums.");
      _logger.Info("### PHASE 3 : Generating adapters");
      var adapters = _adaptersGenerator.Generate(sources, targets, matches);
      _logger.Info("### PHASE 4 : Generating unit tests for adapters");
      var adapterTests = _adapterTestsGenerator.Generate(sources, targets, adapters);
      _logger.Info("### PHASE 5 : Writing");
      _adapterWriter.Write(adapters, outputDirectory);
      _adapterTestsWriter.Write(adapterTests, outputDirectory);
    }
  }
}