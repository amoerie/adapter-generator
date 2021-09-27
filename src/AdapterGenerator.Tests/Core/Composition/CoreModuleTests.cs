using System;
using AdapterGenerator.Core.Composition;
using AdapterGenerator.Core.Generation;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using AdapterGenerator.Core.Logging;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Matching.Enums.Values;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Core.Writing;
using Autofac;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Composition {
  [TestFixture]
  public class CoreModuleTests {
    [TestCase(typeof(ICodeToSyntaxTreeParser))]
    [TestCase(typeof(ITypeDeclarationsFromSyntaxTreeExtractor))]
    [TestCase(typeof(INamespaceDeclarationSyntaxFinder))]
    [TestCase(typeof(IFileParser))]
    [TestCase(typeof(IFileReader))]
    public void ShouldResolveParsingTypes(Type serviceType) {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      new Action(() => container.Resolve(serviceType)).ShouldNotThrow();
    }

    [TestCase(typeof(ILogger))]
    public void ShouldResolveLoggingTypes(Type serviceType) {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      new Action(() => container.Resolve(serviceType)).ShouldNotThrow();
    }


    [TestCase(typeof(IClassMatcher))]
    [TestCase(typeof(ISingleClassMatcher))]
    [TestCase(typeof(IPropertyMatcher))]
    [TestCase(typeof(ISinglePropertyMatcher))]
    [TestCase(typeof(IEnumMatcher))]
    [TestCase(typeof(ISingleEnumMatcher))]
    [TestCase(typeof(IEnumValueMatcher))]
    [TestCase(typeof(ISingleEnumValueMatcher))]
    [TestCase(typeof(IMatcher))]
    public void ShouldResolveMatchingTypes(Type serviceType) {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      new Action(() => container.Resolve(serviceType)).ShouldNotThrow();
    }

    [TestCase(typeof(IAdaptersGenerator))]
    [TestCase(typeof(IAdapterCompilationUnitGenerator))]
    [TestCase(typeof(IAdapterNamespaceGenerator))]
    [TestCase(typeof(IAdapterClassGenerator))]
    [TestCase(typeof(IAdapterClassFieldsGenerator))]
    [TestCase(typeof(IAdapterClassConstructorGenerator))]
    [TestCase(typeof(IAdaptMethodGenerator))]
    [TestCase(typeof(IAdaptMethodBodyGenerator))]
    [TestCase(typeof(IPropertySpecificStatementsGenerator))]
    [TestCase(typeof(IPropertyAdaptingStatementsGeneratorDecider))]
    [TestCase(typeof(ITypeSyntaxAnalyzer))]
    [TestCase(typeof(IPropertyMatchAnalyzerFactory))]
    [TestCase(typeof(IAdaptersBlueprintFactory))]
    [TestCase(typeof(IAdaptersBlueprintFactory))]
    [TestCase(typeof(IAdapterTestsAdaptMethodTestsGenerator))]
    [TestCase(typeof(IAdapterTestsClassFieldsGenerator))]
    [TestCase(typeof(IAdapterTestsClassGenerator))]
    [TestCase(typeof(IAdapterTestsCompilationUnitGenerator))]
    [TestCase(typeof(IAdapterTestsConstructorTestsGenerator))]
    [TestCase(typeof(IAdapterTestsNamespaceGenerator))]
    [TestCase(typeof(IAdapterTestsPropertySpecificTestStatementsGenerator))]
    [TestCase(typeof(IAdapterTestsPropertyTestMethodGenerator))]
    [TestCase(typeof(IAdapterTestsSetupMethodGenerator))]
    [TestCase(typeof(IPropertyTestStatementsGeneratorFactory))]
    [TestCase(typeof(IDummyValueFactory))]
    public void ShouldResolveGenerationTypes(Type serviceType) {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      new Action(() => container.Resolve(serviceType)).ShouldNotThrow();
    }

    [TestCase(typeof(IFileWriter))]
    [TestCase(typeof(IAdapterWriter))]
    [TestCase(typeof(ISyntaxFormatter))]
    public void ShouldResolveWritingTypes(Type serviceType) {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<CoreModule>();
      var container = containerBuilder.Build();
      new Action(() => container.Resolve(serviceType)).ShouldNotThrow();
    }
  }
}