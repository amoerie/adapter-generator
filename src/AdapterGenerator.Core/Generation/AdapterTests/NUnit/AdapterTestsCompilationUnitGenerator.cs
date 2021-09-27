using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public class AdapterTestsCompilationUnitGenerator : IAdapterTestsCompilationUnitGenerator {
    private readonly IAdapterTestsNamespaceGenerator _namespaceGenerator;

    public AdapterTestsCompilationUnitGenerator(IAdapterTestsNamespaceGenerator namespaceGenerator) {
      if (namespaceGenerator == null) throw new ArgumentNullException(nameof(namespaceGenerator));
      _namespaceGenerator = namespaceGenerator;
    }

    private IEnumerable<UsingDirectiveSyntax> CreateUsingDirectives(SyntaxList<UsingDirectiveSyntax> usings) {
      var defaultUsingDirectives = new[] {
        "System", "System.Collections.Generic", "System.Linq",
        "NUnit.Framework", "FakeItEasy", "FluentAssertions",
        "FluentAssertions", "FluentAssertions",
      };
      var adapterUsingDirectives = usings.AsEnumerable().Select(usingDirective => usingDirective.Name.ToFullString());
      var usingDirectives = defaultUsingDirectives.Concat(adapterUsingDirectives)
        .Distinct()
        .OrderBy(s => s)
        .Select(s => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(s)));
      return usingDirectives;
    }

    public CompilationUnitSyntax Generate(IClassAdapterTestsGenerationContext context) {
      var compilationUnit = SyntaxFactory.CompilationUnit(
        new SyntaxList<ExternAliasDirectiveSyntax>(),
        new SyntaxList<UsingDirectiveSyntax>().AddRange(
          CreateUsingDirectives(context.Adapter.CompilationUnitSyntax.Usings)),
        new SyntaxList<AttributeListSyntax>(),
        new SyntaxList<MemberDeclarationSyntax>());
      return compilationUnit.WithMembers(SyntaxFactory.List(new MemberDeclarationSyntax[] {
        _namespaceGenerator.Generate(context.WithCompilationUnit(compilationUnit))
      }));
    }

    public CompilationUnitSyntax Generate(IEnumAdapterTestsGenerationContext context) {
      var compilationUnit = SyntaxFactory.CompilationUnit(
        new SyntaxList<ExternAliasDirectiveSyntax>(),
        new SyntaxList<UsingDirectiveSyntax>().AddRange(
          CreateUsingDirectives(context.Adapter.CompilationUnitSyntax.Usings)),
        new SyntaxList<AttributeListSyntax>(),
        new SyntaxList<MemberDeclarationSyntax>());
      return compilationUnit.WithMembers(SyntaxFactory.List(new MemberDeclarationSyntax[] {
        _namespaceGenerator.Generate(context.WithCompilationUnit(compilationUnit))
      }));
    }
  }
}