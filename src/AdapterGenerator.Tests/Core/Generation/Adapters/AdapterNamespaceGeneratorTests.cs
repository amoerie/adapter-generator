using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FakeItEasy;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters {
  [TestFixture]
  public class AdapterNamespaceGeneratorTests {
    AdapterNamespaceGenerator _sut;
    private IAdapterClassGenerator _classGenerator;

    [SetUp]
    public virtual void SetUp() {
      _classGenerator = _classGenerator.Fake();
      _sut = new AdapterNamespaceGenerator(_classGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterNamespaceGeneratorTests {
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
    public class GenerateNamespaceForClassAdapter : AdapterNamespaceGeneratorTests {
      private IClass _source;
      private IClass _target;
      private IClassAdapterBlueprint _blueprint;
      private IClassMatch _match;
      private IClassAdapterGenerationContextWithCompilationUnit _context;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = TestUtilities.ExtractClasses(TestDataIndex.Core.Source).Single();
        _target = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
        _match = new ClassMatch {
          Source = _source,
          Target = _target,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        _blueprint = _blueprint.Fake();
        _context = _context.Fake();
        A.CallTo(() => _classGenerator.Generate(A<IClassAdapterGenerationContextWithNamespace>._))
          .Returns(SyntaxFactory.ClassDeclaration("TargetAdapter"));
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.ClassMatch).Returns(_match);
      }

      [Test]
      public void ShouldGenerateNamespaceDeclaration() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateNamespaceWithNameEqualToTargetClass() {
        _sut.Generate(_context)
          .Name.ToFullString()
          .Should()
          .Be(_target.NamespaceDeclarationSyntax.Name.ToFullString());
      }
    }

    [TestFixture]
    public class GenerateNamespaceForEnumAdapter : AdapterNamespaceGeneratorTests {
      private IEnum _source;
      private IEnum _target;
      private IEnumAdapterBlueprint _blueprint;
      private IEnumMatch _match;
      private IEnumAdapterGenerationContextWithCompilationUnit _context;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = TestUtilities.ExtractEnums(TestDataIndex.Core.SourceEnum).Single();
        _target = TestUtilities.ExtractEnums(TestDataIndex.Core.TargetEnum).Single();
        _match = new EnumMatch {
          Source = _source,
          Target = _target,
          ValueMatches = ImmutableList<IEnumValueMatch>.Empty
        };
        _blueprint = _blueprint.Fake();
        _context = _context.Fake();
        A.CallTo(() => _classGenerator.Generate(A<IEnumAdapterGenerationContextWithNamespace>._))
          .Returns(SyntaxFactory.ClassDeclaration("TargetAdapter"));
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.EnumMatch).Returns(_match);
      }

      [Test]
      public void ShouldGenerateNamespaceDeclaration() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateNamespaceWithNameEqualToTargetClass() {
        _sut.Generate(_context)
          .Name.ToFullString()
          .Should()
          .Be(_target.NamespaceDeclarationSyntax.Name.ToFullString());
      }
    }
  }
}