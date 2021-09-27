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
  public class AdapterCompilationUnitGeneratorTests {
    AdapterCompilationUnitGenerator _sut;
    IAdapterNamespaceGenerator _namespaceGenerator;

    [SetUp]
    public virtual void SetUp() {
      _namespaceGenerator = _namespaceGenerator.Fake();
      _sut = new AdapterCompilationUnitGenerator(_namespaceGenerator);
    }

    [TestFixture]
    public class Constructor : AdapterCompilationUnitGeneratorTests {
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
    public class GenerateCompilationUnitForClassAdapter : AdapterCompilationUnitGeneratorTests {
      private IClass _sourceClass;
      private IClass _targetClass;
      private IClassMatch _classMatch;
      private IClassAdapterGenerationContext _context;
      private IClassAdapterBlueprint _blueprint;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _sourceClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Source).Single();
        _targetClass = TestUtilities.ExtractClasses(TestDataIndex.Core.Target).Single();
        _classMatch = new ClassMatch {
          Source = _sourceClass,
          Target = _targetClass,
          PropertyMatches = ImmutableList<IPropertyMatch>.Empty
        };
        _blueprint = _blueprint.Fake();
        _context = _context.Fake();
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.ClassMatch).Returns(_classMatch);
        A.CallTo(() => _namespaceGenerator.Generate(A<IClassAdapterGenerationContextWithCompilationUnit>._))
          .Returns(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("Hello")));
      }

      [Test]
      public void ShouldGenerateCompilationUnit() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldAddAllDefaultUsingStatements() {
        var defaultUsingNamespaces = new[] {"System", "System.Collections.Generic", "System.Linq"};
        var namespaces = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        namespaces.Should().Contain(defaultUsingNamespaces);
      }

      [Test]
      public void ShouldAddAllUsingStatementsOfSource() {
        var usings = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        var sourceUsings = _sourceClass.CompilationUnitSyntax.Usings.Select(u => u.Name.ToFullString());
        usings.Should().Contain(sourceUsings);
      }

      [Test]
      public void ShouldAddAllUsingStatementsOfTarget() {
        var usings = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        var targetUsings = _targetClass.CompilationUnitSyntax.Usings.Select(u => u.Name.ToFullString());
        usings.Should().Contain(targetUsings);
      }

      [Test]
      public void ShouldGenerateCorrectCompilationUnit() {
        TestUtilities.FormatCode(_sut.Generate(_context))
          .ShouldBeSameCodeAs(@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello
{
}");
      }
    }

    [TestFixture]
    public class GenerateCompilationUnitForEnumAdapter : AdapterCompilationUnitGeneratorTests {
      private IEnum _source;
      private IEnum _target;
      private IEnumMatch _match;
      private IEnumAdapterGenerationContext _context;
      private IEnumAdapterBlueprint _blueprint;

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
        A.CallTo(() => _context.Blueprint).Returns(_blueprint);
        A.CallTo(() => _blueprint.EnumMatch).Returns(_match);
        A.CallTo(() => _namespaceGenerator.Generate(A<IEnumAdapterGenerationContextWithCompilationUnit>._))
          .Returns(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("Hello")));
      }

      [Test]
      public void ShouldGenerateCompilationUnit() {
        _sut.Generate(_context).Should().NotBeNull();
      }

      [Test]
      public void ShouldAddAllDefaultUsingStatements() {
        var defaultUsingNamespaces = new[] {"System", "System.Collections.Generic", "System.Linq"};
        var namespaces = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        namespaces.Should().Contain(defaultUsingNamespaces);
      }

      [Test]
      public void ShouldAddAllUsingStatementsOfSource() {
        var usings = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        var sourceUsings = _source.CompilationUnitSyntax.Usings.Select(u => u.Name.ToFullString());
        usings.Should().Contain(sourceUsings);
      }

      [Test]
      public void ShouldAddAllUsingStatementsOfTarget() {
        var usings = _sut.Generate(_context).Usings.Select(u => u.Name.ToFullString());
        var targetUsings = _target.CompilationUnitSyntax.Usings.Select(u => u.Name.ToFullString());
        usings.Should().Contain(targetUsings);
      }

      [Test]
      public void ShouldGenerateCorrectCompilationUnit() {
        TestUtilities.FormatCode(_sut.Generate(_context))
          .ShouldBeSameCodeAs(@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello
{
}");
      }
    }
  }
}