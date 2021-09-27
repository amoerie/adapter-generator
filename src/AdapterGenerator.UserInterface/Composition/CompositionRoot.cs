using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using AdapterGenerator.UserInterface.GeneratorModule.Handlers;
using Autofac;

namespace AdapterGenerator.UserInterface.Composition {
  public class CompositionRoot {
    public static void Compose(ContainerBuilder builder) {
      builder.RegisterModule<WritingModule>();
      builder.RegisterModule<ParsingModule>();
      builder.RegisterModule<MatchingModule>();
      builder.RegisterModule<GenerationModule>();
      builder.RegisterModule<LoggingModule>();

      builder.RegisterType<AdapterGeneratorService>().AsImplementedInterfaces().SingleInstance();

      builder.RegisterType<FileOrFolderHandler>().AsImplementedInterfaces().SingleInstance();
    }
  }
}