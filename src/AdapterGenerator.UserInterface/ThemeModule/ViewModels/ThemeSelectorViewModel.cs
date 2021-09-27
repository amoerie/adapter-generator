using System.Collections.Generic;
using Prism.Mvvm;

namespace AdapterGenerator.UserInterface.ThemeModule.ViewModels {
  public class ThemeSelectorViewModel : BindableBase {
    public List<string> Accents { get; set; }
    public List<ThemeSystem.ThemeOption> Themes { get; set; }

    public string SelectedAccent {
      get { return ThemeSystem.SelectedAccent; }
      set { SetAccent(value); }
    }

    public ThemeSystem.ThemeOption SelectedTheme {
      get { return ThemeSystem.SelectedTheme; }
      set { SetTheme(value); }
    }

    private void SetAccent(string value) {
      ThemeSystem.SelectedAccent = value;
      OnPropertyChanged(() => SelectedAccent);
      ThemeSystem.ChangeApplicationStyle();
    }

    private void SetTheme(ThemeSystem.ThemeOption value) {
      ThemeSystem.SelectedTheme = value;
      OnPropertyChanged(() => SelectedTheme);
      ThemeSystem.ChangeApplicationStyle();
    }

    public ThemeSelectorViewModel() {
      Accents = ThemeSystem.Accents;
      Themes = ThemeSystem.Themes;
    }
  }
}