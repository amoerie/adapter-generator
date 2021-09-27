using AdapterGenerator.UserInterface.GeneratorModule.Views;
using Prism.Modularity;
using Prism.Regions;

namespace AdapterGenerator.UserInterface.GeneratorModule {
  public class GeneratorModule : IModule {
    readonly IRegionManager _regionManager;

    public GeneratorModule(IRegionManager regionManager) {
      _regionManager = regionManager;
    }

    public void Initialize() {
      _regionManager.RegisterViewWithRegion("MainRegion", typeof(Generator));
      _regionManager.RegisterViewWithRegion("SourceFileSelectRegion", typeof(SourceFileSelect));
      _regionManager.RegisterViewWithRegion("TargetFileSelectRegion", typeof(TargetFileSelect));
      _regionManager.RegisterViewWithRegion("GeneratorLogRegion", typeof(GeneratorLog));
    }
  }
}