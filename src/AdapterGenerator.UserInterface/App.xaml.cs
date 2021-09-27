using System.Linq;
using System.Windows;
using AdapterGenerator.UserInterface.Composition;
using AdapterGenerator.UserInterface.ThemeModule;

namespace AdapterGenerator.UserInterface {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App {
    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

      var bootstrapper = new Bootstrapper();

      ThemeSystem.SelectedAccent = Settings.Default.Accent;
      ThemeSystem.SelectedTheme =
        ThemeSystem.Themes.Single(t => t.Value == Settings.Default.Theme);
      ThemeSystem.ChangeApplicationStyle();

      bootstrapper.Run();
    }

    protected override void OnExit(ExitEventArgs e) {
      Settings.Default.Accent = ThemeSystem.SelectedAccent;
      Settings.Default.Theme = ThemeSystem.SelectedTheme.Value;
      Settings.Default.Save();

      base.OnExit(e);
    }
  }
}