using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsCompilationUnitGeneratorTests {
    AdapterTestsCompilationUnitGenerator _sut;
    IAdapterTestsNamespaceGenerator _namespaceGenerator;

    [SetUp]
    public virtual void SetUp() {
      _namespaceGenerator = _namespaceGenerator.Fake();
      _sut = new AdapterTestsCompilationUnitGenerator(_namespaceGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterTestsCompilationUnitGeneratorTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldHaveNoOptionalDependencies() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class GenerateCompilationUnitForClassAdapter : AdapterTestsCompilationUnitGeneratorTests {
      private IClassAdapterTestsGenerationContext _context;
      private IGeneratedClassAdapter _adapter;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _adapter = _adapter.Fake();
        _context = _context.Fake();
        A.CallTo(() => _context.Adapter).Returns(_adapter);
        A.CallTo(() => _adapter.CompilationUnitSyntax)
          .Returns(
            SyntaxFactory.CompilationUnit()
              .WithUsings(
                SyntaxFactory.SingletonList(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("AdapterUsing")))));
        A.CallTo(() => _namespaceGenerator.Generate(A<IClassAdapterTestsGenerationContextWithCompilationUnit>._))
          .Returns(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("Hello")));
      }

      [Test]
      public void ShouldGenerateCompilationUnit() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldAddAllDefaultUsingStatements() {
        var defaultUsingNamespaces = new[] {
          "System", "System.Collections.Generic", "System.Linq",
          "NUnit.Framework", "FakeItEasy", "FluentAssertions",
          "FluentAssertions", "FluentAssertions"
        };
        ;
        var namespaces = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        namespaces.Should().Contain(defaultUsingNamespaces);
      }

      [Test]
      public void ShouldAddAllUsingStatementsOfAdapter() {
        var usings = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        var adapterUsings = _adapter.CompilationUnitSyntax.Usings.Select(u => u.Name.ToFullString());
        usings.Should().Contain(adapterUsings);
      }

      [Test]
      public void ShouldGenerateCorrectCompilationUnit() {
        TestUtilities.FormatCode(_sut.Generate(_context))
          .ShouldBeSameCodeAs(@"using AdapterUsing;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello
{
}");
      }
    }
  }
}