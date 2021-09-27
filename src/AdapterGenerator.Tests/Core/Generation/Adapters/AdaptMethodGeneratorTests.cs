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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Generation.Adapters {
  [TestFixture]
  public class AdaptMethodGeneratorTests {
    AdaptMethodGenerator _sut;
    private IAdaptMethodBodyGenerator _bodyGenerator;

    [SetUp]
    public virtual void SetUp() {
      _bodyGenerator = _bodyGenerator.Fake();
      _sut = new AdaptMethodGenerator(_bodyGenerator);
    }

    [TestFixture]
    public class Constructor : AdaptMethodGeneratorTests {
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
    public class GenerateAdaptMethodForClass : AdaptMethodGeneratorTests {
      private IClass _source;
      private IClass _target;
      private IClassMatch _match;
      private IAdapterClassGenerator _classGenerator;
      private IClassAdapterGenerationContextWithClass _context;
      private IClassAdapterBlueprint _blueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = TestUtilities.ExtractClasses(TestDataIndex.Core.Source).Single();
        _target = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
        _classGenerator = _classGenerator.Fake();
        _match = new ClassMatch {
          Source = _source,
          Target = _target,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        _context = _context.Fake();
        _blueprint = _blueprint.Fake();
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.ClassMatch).Returns(_match);
      }

      [Test]
      public void ShouldGenerateAMethod() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateAMethodWithNameAdapt() {
        _sut.Generate(_context).Identifier.Text.Should().Be("Adapt");
      }

      [Test]
      public void ShouldGenerateAMethodThatIsPublic() {
        _sut.Generate(_context)
          .Modifiers.Should()
          .Contain(token => token.Kind() == SyntaxKind.PublicKeyword);
      }

      [Test]
      public void ShouldGenerateAMethodWithBodyDeterminedByTheBodyGenerator() {
        BlockSyntax methodBody = SyntaxFactory.Block(SyntaxFactory.ReturnStatement());
        A.CallTo(() => _bodyGenerator.Generate(_context)).Returns(methodBody);
        _sut.Generate(_context).Body.ToFullString().Should().Be(methodBody.ToFullString());
      }

      [Test]
      public void ShouldGenerateCorrectMethod() {
        TestUtilities.FormatCode(_sut.Generate(_context))
          .ShouldBeSameCodeAs(
            "public AdapterGenerator.Tests.Core.Generation.Adapters.TestData.Target Adapt(AdapterGenerator.Tests.Core.Generation.Adapters.TestData.Source source)");
      }
    }

    [TestFixture]
    public class GenerateAdaptMethodForEnum : AdaptMethodGeneratorTests {
      private IEnum _source;
      private IEnum _target;
      private IEnumMatch _match;
      private IAdapterClassGenerator _classGenerator;
      private IEnumAdapterGenerationContextWithClass _context;
      private IEnumAdapterBlueprint _blueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _source = TestUtilities.ExtractEnums(TestDataIndex.Core.SourceEnum).Single();
        _target = TestUtilities.ExtractEnums(TestDataIndex.Core.TargetEnum).Single();
        _classGenerator = _classGenerator.Fake();
        _match = new EnumMatch {
          Source = _source,
          Target = _target,
          ValueMatches = ImmutableList<IEnumValueMatch>.Empty
        };
        _context = _context.Fake();
        _blueprint = _blueprint.Fake();
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.EnumMatch).Returns(_match);
      }

      [Test]
      public void ShouldGenerateAMethod() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldGenerateAMethodWithNameAdapt() {
        _sut.Generate(_context).Identifier.Text.Should().Be("Adapt");
      }

      [Test]
      public void ShouldGenerateAMethodThatIsPublic() {
        _sut.Generate(_context)
          .Modifiers.Should()
          .Contain(token => token.Kind() == SyntaxKind.PublicKeyword);
      }

      [Test]
      public void ShouldGenerateAMethodWithBodyDeterminedByTheBodyGenerator() {
        BlockSyntax methodBody = SyntaxFactory.Block(SyntaxFactory.ReturnStatement());
        A.CallTo(() => _bodyGenerator.Generate(_context)).Returns(methodBody);
        _sut.Generate(_context).Body.ToFullString().Should().Be(methodBody.ToFullString());
      }

      [Test]
      public void ShouldGenerateCorrectMethod() {
        TestUtilities.FormatCode(_sut.Generate(_context))
          .ShouldBeSameCodeAs(
            "public AdapterGenerator.Tests.Core.Generation.Adapters.TestData.TargetEnum Adapt(AdapterGenerator.Tests.Core.Generation.Adapters.TestData.SourceEnum source)");
      }
    }
  }
}