using System;
using AdapterGenerator.Core.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AdapterGenerator.Core.Parsing {
  public class CodeToSyntaxTreeParser : ICodeToSyntaxTreeParser {
    private readonly ILogger _logger;

    public CodeToSyntaxTreeParser(ILogger logger) {
      if (logger == null) throw new ArgumentNullException(nameof(logger));
      _logger = logger;
    }

    public SyntaxTree Parse(string code) {
      _logger.Info($"Parsing {code?.Split('\n').Length} lines of code");
      return CSharpSyntaxTree.ParseText(code);
    }
  }
}