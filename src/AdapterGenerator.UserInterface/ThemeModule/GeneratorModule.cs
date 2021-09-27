using AdapterGenerator.UserInterface.ThemeModule.Views;
using Prism.Modularity;
using Prism.Regions;

namespace AdapterGenerator.UserInterface.ThemeModule {
  public class ThemeSelectorModule : IModule {
    readonly IRegionManager _regionManager;

    public ThemeSelectorModule(IRegionManager regionManager) {
      _regionManager = regionManager;
    }

    public void Initialize() {
      _regionManager.RegisterViewWithRegion("ThemeRegion", typeof(ThemeSelector));
    }
  }
}