using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.Net;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace AdapterGenerator.Tests {
  internal static class TestUtilities {
    public static IClass ParseClass(string @class) {
      var syntaxTree = CSharpSyntaxTree.ParseText(@class);
      var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
      var classDeclaration = compilationUnitSyntax.Members.OfType<ClassDeclarationSyntax>().Single();
      return new RealClass(compilationUnitSyntax, null, classDeclaration);
    }

    public static IEnum ParseEnum(string @enum) {
      var syntaxTree = CSharpSyntaxTree.ParseText(@enum);
      var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
      var enumDeclaration = compilationUnitSyntax.Members.OfType<EnumDeclarationSyntax>().Single();
      return new RealEnum(compilationUnitSyntax, null, enumDeclaration);
    }

    public static IImmutableList<IClass> ExtractClasses(FileInfo file) {
      var logger = new ConsoleLogger();
      var fileReader = new FileReader(logger);
      var codeParser = new CodeToSyntaxTreeParser(logger);
      var extractor = new TypeDeclarationsFromSyntaxTreeExtractor(logger, new NamespaceDeclarationSyntaxFinder());
      return extractor.Extract(codeParser.Parse(fileReader.ReadFile(file.FullName))).Classes;
    }

    public static IImmutableList<IEnum> ExtractEnums(FileInfo file) {
      var logger = new ConsoleLogger();
      var fileReader = new FileReader(logger);
      var codeParser = new CodeToSyntaxTreeParser(logger);
      var extractor = new TypeDeclarationsFromSyntaxTreeExtractor(logger, new NamespaceDeclarationSyntaxFinder());
      return extractor.Extract(codeParser.Parse(fileReader.ReadFile(file.FullName))).Enums;
    }

    public static string FormatCode(SyntaxNode syntax) {
      return Formatter.Format(syntax, SyntaxAnnotation.ElasticAnnotation, new AdhocWorkspace()).ToFullString();
    }

    public static void ShouldBeSameCodeAs(this string code, string expectedCode) {
      var splitCode = code.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);
      var splitExpectedCode = expectedCode.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);
      Console.WriteLine("---- ACTUAL CODE -----");
      splitCode.ForEach(line => Console.WriteLine((string)line));
      Console.WriteLine("---- EXPECTED CODE ---");
      splitExpectedCode.ForEach(line => Console.WriteLine(line));
      splitCode.ShouldBeEquivalentTo(splitExpectedCode);
    }
  }
}