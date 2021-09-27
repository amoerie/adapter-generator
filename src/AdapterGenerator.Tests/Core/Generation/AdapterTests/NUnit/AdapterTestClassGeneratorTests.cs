using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AdapterGenerator.Tests.Core.Generation.AdapterTests.NUnit {
  [TestFixture]
  public class AdapterTestClassGeneratorTests {
    AdapterTestsClassGenerator _sut;
    private IAdapterTestsClassFieldsGenerator _fieldsGenerator;
    private IAdapterTestsSetupMethodGenerator _setupMethodGenerator;
    private IAdapterTestsConstructorTestsGenerator _constructorTestsGenerator;
    private IAdapterTestsAdaptMethodTestsGenerator _adaptMethodTestsGenerator;

    [SetUp]
    public virtual void SetUp() {
      _fieldsGenerator = _fieldsGenerator.Fake();
      _setupMethodGenerator = _setupMethodGenerator.Fake();
      _constructorTestsGenerator = _constructorTestsGenerator.Fake();
      _adaptMethodTestsGenerator = _adaptMethodTestsGenerator.Fake();
      _sut = new AdapterTestsClassGenerator(_fieldsGenerator, _setupMethodGenerator, _constructorTestsGenerator,
        _adaptMethodTestsGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterTestClassGeneratorTests {
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
    public class GenerateClass : AdapterTestClassGeneratorTests {
      private IClassAdapterTestsGenerationContextWithNamespace _context;
      private IGeneratedClassAdapter _adapter;
      private IClassAdapterBlueprint _blueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _context = _context.Fake();
        _adapter = _adapter.Fake();
        _blueprint = _blueprint.Fake();
        var simpleAdapter = TestUtilities.ExtractClasses(TestDataIndex.Core.SimpleAdapter).Single();
        A.CallTo(() => _context.Adapter).Returns(_adapter);
        A.CallTo(() => _context.TestClassName).Returns("SimpleAdapterTests");
        A.CallTo(() => _adapter.Blueprint).Returns(_blueprint);
        A.CallTo(() => _adapter.CompilationUnitSyntax).Returns(simpleAdapter.CompilationUnitSyntax);
        A.CallTo(() => _blueprint.QualifiedName).Returns(IdentifierName("SimpleAdapter"));

        A.CallTo(() => _fieldsGenerator.Generate(A<IClassAdapterTestsGenerationContextWithClass>._))
          .Returns(ImmutableList.Create(FieldDeclaration(
            VariableDeclaration(IdentifierName("SimpleAdapter"))
              .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier("_sut")))))
            .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword)))));
        A.CallTo(() => _setupMethodGenerator.Generate(A<IClassAdapterTestsGenerationContextWithClass>._))
          .Returns(
            MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("Setup")).WithBody(Block()));
        A.CallTo(() => _constructorTestsGenerator.Generate(A<IClassAdapterTestsGenerationContextWithClass>._))
          .Returns(ClassDeclaration("Constructor"));
        A.CallTo(() => _adaptMethodTestsGenerator.Generate(A<IClassAdapterTestsGenerationContextWithClass>._))
          .Returns(ClassDeclaration("Adapt"));
      }

      [Test]
      public void ShouldGenerateClass() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateCorrectClass() {
        var code = TestUtilities.FormatCode(_sut.Generate(_context));
        code.ShouldBeSameCodeAs(@"[TestFixture]
public class SimpleAdapterTests
{
    private SimpleAdapter _sut;

    void Setup()
    {
    }

    class Constructor
    {
    }

    class Adapt
    {
    }
}");
      }
    }
  }
}