using Autofac;

namespace AdapterGenerator.Core.Composition {
  public class CoreModule : Module {
    protected override void Load(ContainerBuilder builder) {
      builder.RegisterModule<LoggingModule>();
      builder.RegisterModule<ParsingModule>();
      builder.RegisterModule<MatchingModule>();
      builder.RegisterModule<GenerationModule>();
      builder.RegisterModule<WritingModule>();
      builder.RegisterType<AdapterGeneratorService>().AsImplementedInterfaces().SingleInstance();
    }
  }
}