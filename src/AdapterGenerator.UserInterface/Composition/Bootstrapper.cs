using System.Collections.Generic;
using System.Windows;
using Autofac;

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
      base.ConfigureModuleCatalog(moduleCatalog);
      moduleCatalog.AddModule<MyModule>();

      Bootstrapper.Run(this, new List<AutofacModuleBase>()
      {
        // autofac modules
      });
    }
  }
}