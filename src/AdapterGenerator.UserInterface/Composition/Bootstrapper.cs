using System.Windows;
using Autofac;
using Prism.Autofac;
using Prism.Modularity;

namespace AdapterGenerator.UserInterface.Composition {
  public class Bootstrapper : AutofacBootstrapper {
    protected override DependencyObject CreateShell() {
      return Container.Resolve<MainWindow>();
    }

    protected override void InitializeShell() {
      Application.Current.MainWindow.Show();
    }

    protected override void ConfigureContainerBuilder(ContainerBuilder builder) {
      CompositionRoot.Compose(builder);
      base.ConfigureContainerBuilder(builder);
    }

    protected override void ConfigureModuleCatalog() {
      var catalog = (ModuleCatalog) ModuleCatalog;
      catalog.AddModule(typeof(GeneratorModule.GeneratorModule));
      catalog.AddModule(typeof(ThemeModule.ThemeSelectorModule));
    }
  }
}