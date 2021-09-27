using AdapterGenerator.UserInterface.GeneratorModule.ViewModels;
using Autofac;

namespace AdapterGenerator.UserInterface.Composition {
  public class LoggingModule : Module {
    protected override void Load(ContainerBuilder builder) {
      builder.RegisterType<GeneratorLogger>().AsImplementedInterfaces().SingleInstance();
    }
  }
}