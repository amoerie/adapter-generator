using System.Windows;
using System.Windows.Media;
using AdapterGenerator.UserInterface.GeneratorModule.ViewModels;
using Microsoft.Win32;

namespace AdapterGenerator.UserInterface.GeneratorModule.Views {
  /// <summary>
  /// Interaction logic for TargetFileSelect.xaml
  /// </summary>
  public partial class TargetFileSelect {
    public TargetFileSelect() {
      InitializeComponent();
    }

    private void TargetListView_Drop(object sender, DragEventArgs e) {
      TargetListView.ClearValue(BackgroundProperty);
      if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

      // Note that you can have more than one file.
      var paths = (string[]) e.Data.GetData(DataFormats.FileDrop);

      var viewModel = DataContext as TargetFileSelectViewModel;
      // Assuming you have one file that you care about, pass it off to whatever
      // handling code you have defined.
      foreach (var fileOrFolder in paths) {
        viewModel?.AddTargetFile(fileOrFolder);
      }
    }

    private void TargetListView_DragOver(object sender, DragEventArgs e) {
      e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void TargetListView_DragEnter(object sender, DragEventArgs e) {
      TargetListView.Background = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#CC647687"));
    }

    private void TargetListView_DragLeave(object sender, DragEventArgs e) {
      TargetListView.ClearValue(BackgroundProperty);
    }

    private void AddTargetFiles_Click(object sender, RoutedEventArgs e) {
      var openFileDialog = new OpenFileDialog {
        Multiselect = true,
        Filter = "C# files (*.cs)|*.cs"
      };
      if (openFileDialog.ShowDialog() != true) return;

      var viewModel = DataContext as TargetFileSelectViewModel;
      foreach (var file in openFileDialog.FileNames) {
        viewModel?.AddTargetFile(file);
      }
    }
  }
}