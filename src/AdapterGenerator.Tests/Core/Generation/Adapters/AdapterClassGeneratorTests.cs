using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters {
  [TestFixture]
  public class AdapterClassGeneratorTests {
    AdapterClassGenerator _sut;
    private IAdaptMethodGenerator _methodGenerator;
    private IAdapterClassFieldsGenerator _fieldsGenerator;
    private IAdapterClassConstructorGenerator _constructorGenerator;

    [SetUp]
    public virtual void SetUp() {
      _fieldsGenerator = _fieldsGenerator.Fake();
      _constructorGenerator = _constructorGenerator.Fake();
      _methodGenerator = _methodGenerator.Fake();
      _sut = new AdapterClassGenerator(_fieldsGenerator, _constructorGenerator, _methodGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterClassGeneratorTests {
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
    public class GenerateClassForClassAdapter : AdapterClassGeneratorTests {
      private IClassAdapterGenerationContextWithNamespace _classAdapterGenerationContext;
      private IClassAdapterBlueprint _adapterBlueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _classAdapterGenerationContext = _classAdapterGenerationContext.Fake();
        _adapterBlueprint = _adapterBlueprint.Fake();
        A.CallTo(() => _classAdapterGenerationContext.Blueprint).Returns(_adapterBlueprint);
        A.CallTo(() => _adapterBlueprint.Name).Returns(SyntaxFactory.IdentifierName("TargetAdapter"));
        A.CallTo(() => _adapterBlueprint.InterfaceType).Returns(SyntaxFactory.GenericName("ITargetAdapter"));
        A.CallTo(() => _methodGenerator.Generate(A<IClassAdapterGenerationContextWithClass>._))
          .Returns(SyntaxFactory.MethodDeclaration(
            SyntaxFactory.IdentifierName("Target"), "Adapt")
            .WithBody(SyntaxFactory.Block()));
        A.CallTo(() => _constructorGenerator.Generate(A<IClassAdapterGenerationContextWithClass>._))
          .Returns(SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier("TargetAdapter")));
        A.CallTo(() => _fieldsGenerator.Generate(A<IClassAdapterGenerationContextWithClass>._))
          .Returns(ImmutableList<FieldDeclarationSyntax>.Empty);
      }

      [Test]
      public void ShouldGenerateAClassDeclaration() {
        _sut.Generate(_classAdapterGenerationContext).Should().NotBeNull();
      }

      [Test]
      public void ShouldGeneratePublicClass() {
        _sut.Generate(_classAdapterGenerationContext)
          .Modifiers.Should()
          .Contain(token => token.Kind() == SyntaxKind.PublicKeyword);
      }

      [Test]
      public void ShouldGenerateClassThatImplementsIAdapter() {
        var interfaceSyntax = _sut.Generate(_classAdapterGenerationContext).BaseList.Types.Single();
        interfaceSyntax.Type.Should().BeOfType<GenericNameSyntax>();
        var iAdapterSyntax = interfaceSyntax.Type as GenericNameSyntax;
        iAdapterSyntax.Should().NotBeNull();
        iAdapterSyntax.Identifier.Text.Should().Be("ITargetAdapter");
      }
    }

    [TestFixture]
    public class GenerateClassForEnumAdapter : AdapterClassGeneratorTests {
      private IEnumAdapterGenerationContextWithNamespace _context;
      private IEnumAdapterBlueprint _blueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _blueprint = _blueprint.Fake();
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.Name).Returns(SyntaxFactory.IdentifierName("TargetAdapter"));
        A.CallTo(() => _blueprint.InterfaceType).Returns(SyntaxFactory.GenericName("ITargetAdapter"));
        A.CallTo(() => _methodGenerator.Generate(A<IEnumAdapterGenerationContextWithClass>._))
          .Returns(SyntaxFactory.MethodDeclaration(
            SyntaxFactory.IdentifierName("Target"), "Adapt")
            .WithBody(SyntaxFactory.Block()));
      }

      [Test]
      public void ShouldGenerateAClassDeclaration() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGeneratePublicClass() {
        _sut.Generate(_context)
          .Modifiers.Should()
          .Contain(token => token.Kind() == SyntaxKind.PublicKeyword);
      }

      [Test]
      public void ShouldGenerateClassThatImplementsIAdapter() {
        var interfaceSyntax = _sut.Generate(_context).BaseList.Types.Single();
        interfaceSyntax.Type.Should().BeOfType<GenericNameSyntax>();
        var iAdapterSyntax = interfaceSyntax.Type as GenericNameSyntax;
        iAdapterSyntax.Should().NotBeNull();
        iAdapterSyntax.Identifier.Text.Should().Be("ITargetAdapter");
      }
    }
  }
}