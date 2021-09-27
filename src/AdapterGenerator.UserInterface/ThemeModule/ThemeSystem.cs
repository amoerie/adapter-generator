using System.Collections.Generic;
using System.Windows;
using MahApps.Metro;

namespace AdapterGenerator.UserInterface.ThemeModule {
  public static class ThemeSystem {
    public const string DefaultAccent = "Steel";
    public static readonly ThemeOption DefaultTheme = new ThemeOption {DisplayValue = "Dark", Value = "BaseDark"};

    public static string SelectedAccent { get; set; }
    public static ThemeOption SelectedTheme { get; set; }

    public static List<string> Accents { get; set; } = new List<string> {
      "Red",
      "Green",
      "Blue",
      "Purple",
      "Orange",
      "Lime",
      "Emerald",
      "Teal",
      "Cyan",
      "Cobalt",
      "Indigo",
      "Violet",
      "Pink",
      "Magenta",
      "Crimson",
      "Amber",
      "Yellow",
      "Brown",
      "Olive",
      "Steel",
      "Mauve",
      "Taupe",
      "Sienna"
    };

    public static List<ThemeOption> Themes { get; set; } = new List<ThemeOption> {
      new ThemeOption {DisplayValue = "Light", Value = "BaseLight"},
      DefaultTheme
    };

    public static void ChangeApplicationStyle() {
      ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(SelectedAccent),
        ThemeManager.GetAppTheme(SelectedTheme.Value));
    }

    public class ThemeOption {
      public string DisplayValue { get; set; }
      public string Value { get; set; }
    }
  }
}