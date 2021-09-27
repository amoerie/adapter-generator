using AdapterGenerator.Core.Parsing;
using Autofac;

namespace AdapterGenerator.Core.Composition {
  public class ParsingModule : Module {
    protected override void Load(ContainerBuilder builder) {
      builder.RegisterType<FileReader>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<CodeToSyntaxTreeParser>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<NamespaceDeclarationSyntaxFinder>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<TypeDeclarationsFromSyntaxTreeExtractor>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<FileParser>().AsImplementedInterfaces().SingleInstance();
    }
  }
}