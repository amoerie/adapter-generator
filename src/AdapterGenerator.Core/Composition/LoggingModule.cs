using AdapterGenerator.Core.Logging;
using Autofac;

namespace AdapterGenerator.Core.Composition {
  public class LoggingModule : Module {
    protected override void Load(ContainerBuilder builder) {
      builder.RegisterType<ConsoleLogger>().AsImplementedInterfaces().SingleInstance();
    }
  }
}