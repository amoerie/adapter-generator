using System.Linq;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestsNamespaceGeneratorTests {
    AdapterTestsNamespaceGenerator _sut;
    private IAdapterTestsClassGenerator _classGenerator;

    [SetUp]
    public virtual void SetUp() {
      _classGenerator = _classGenerator.Fake();
      _sut = new AdapterTestsNamespaceGenerator(_classGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterTestsNamespaceGeneratorTests {
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
    public class GenerateNamespaceForClassAdapter : AdapterTestsNamespaceGeneratorTests {
      private IClassAdapterTestsGenerationContextWithCompilationUnit _context;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        A.CallTo(() => _context.Adapter.CompilationUnitSyntax)
          .Returns(
            SyntaxFactory.CompilationUnit()
              .WithMembers(
                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                  SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("AdapterNamespace")))));
      }

      [Test]
      public void ShouldGenerateCompilationUnit() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateNamespaceDeclaration() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateNamespaceWithNameEqualToAdapter() {
        var adapterNamespace = _context.Adapter.CompilationUnitSyntax
          .Members.OfType<NamespaceDeclarationSyntax>()
          .Single();
        _sut.Generate(_context)
          .Name.ToFullString()
          .Should()
          .Be(adapterNamespace.Name.ToFullString());
      }
    }
  }
}