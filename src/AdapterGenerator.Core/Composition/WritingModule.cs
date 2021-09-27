using AdapterGenerator.Core.Writing;
using Autofac;

namespace AdapterGenerator.Core.Composition {
  public class WritingModule : Module {
    protected override void Load(ContainerBuilder builder) {
      builder.RegisterType<FileWriter>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<SyntaxFormatter>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterWriter>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<AdapterTestsWriter>().AsImplementedInterfaces().SingleInstance();
    }
  }
}