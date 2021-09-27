using System.Linq;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Parsing {
  [TestFixture]
  public class NamespaceDeclarationSyntaxFinderTests {
    NamespaceDeclarationSyntaxFinder _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new NamespaceDeclarationSyntaxFinder();
    }

    [TestFixture]
    public class Constructor : NamespaceDeclarationSyntaxFinderTests {
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
    public class FindNamespace : NamespaceDeclarationSyntaxFinderTests {
      private IClass _personClass;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _personClass = TestUtilities.ExtractClasses(TestDataIndex.Core.OneClass).Single();
      }

      [Test]
      public void ShouldCorrectlyFindNamespace() {
        var namespaceDeclaration = _sut.FindNamespace(_personClass.ClassDeclarationSyntax);
        namespaceDeclaration.Name.ToFullString().Trim()
          .Should()
          .Be("AdapterGenerator.Tests.Core.Analysis.TestData");
      }
    }
  }
}