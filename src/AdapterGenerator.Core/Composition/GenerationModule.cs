using AdapterGenerator.Core.Generation;
using AdapterGenerator.Core.Generation.Adapters;
using AdapterGenerator.Core.Generation.Adapters.Blueprints;
using AdapterGenerator.Core.Generation.Adapters.Properties;
using AdapterGenerator.Core.Generation.AdapterTests;
using AdapterGenerator.Core.Generation.AdapterTests.NUnit;
using Autofac;

namespace AdapterGenerator.Core.Composition {
  public class GenerationModule : Module {
    protected override void Load(ContainerBuilder builder) {
      // Adapters
      builder.RegisterType<Generation.Adapters.AdaptersGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterCompilationUnitGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterNamespaceGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterClassGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterClassFieldsGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterClassConstructorGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdaptMethodGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdaptMethodBodyGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<PropertySpecificStatementsGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<PropertyAdaptingStatementsGeneratorDecider>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<TypeSyntaxAnalyzer>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<PropertyMatchAnalyzerFactory>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdaptersBlueprintFactory>().AsImplementedInterfaces().SingleInstance();

      // Adapter tests
      builder.RegisterType<AdapterTestsGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsAdaptMethodTestsGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsClassFieldsGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsClassGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsCompilationUnitGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsConstructorTestsGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsNamespaceGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsPropertySpecificTestStatementsGenerator>()
        .AsImplementedInterfaces()
        .SingleInstance();
      builder.RegisterType<AdapterTestsPropertyTestMethodGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsSetupMethodGenerator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<PropertyTestStatementsGeneratorFactory>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<DummyValueFactory>().AsImplementedInterfaces().SingleInstance();
    }
  }
}