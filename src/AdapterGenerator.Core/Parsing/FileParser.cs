using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdapterGenerator.Core.Parsing {
  public class FileParser : IFileParser {
    private readonly IFileReader _fileReader;
    private readonly ICodeToSyntaxTreeParser _codeToSyntaxTreeParser;
    private readonly ITypeDeclarationsFromSyntaxTreeExtractor _typeDeclarationsFromSyntaxTreeExtractor;

    public FileParser(IFileReader fileReader, ICodeToSyntaxTreeParser codeToSyntaxTreeParser,
      ITypeDeclarationsFromSyntaxTreeExtractor typeDeclarationsFromSyntaxTreeExtractor) {
      if (fileReader == null) throw new ArgumentNullException(nameof(fileReader));
      if (codeToSyntaxTreeParser == null) throw new ArgumentNullException(nameof(codeToSyntaxTreeParser));
      if (typeDeclarationsFromSyntaxTreeExtractor == null)
        throw new ArgumentNullException(nameof(typeDeclarationsFromSyntaxTreeExtractor));
      _fileReader = fileReader;
      _codeToSyntaxTreeParser = codeToSyntaxTreeParser;
      _typeDeclarationsFromSyntaxTreeExtractor = typeDeclarationsFromSyntaxTreeExtractor;
    }

    public ITypeDeclarations ParseFiles(IEnumerable<FileInfo> files) {
      return files
        .Select(file => file.FullName)
        .Select(_fileReader.ReadFile)
        .Select(_codeToSyntaxTreeParser.Parse)
        .Select(_typeDeclarationsFromSyntaxTreeExtractor.Extract)
        .Aggregate((allTypeDeclarations, nextDeclarations) => allTypeDeclarations.Union(nextDeclarations));
    }
  }
}