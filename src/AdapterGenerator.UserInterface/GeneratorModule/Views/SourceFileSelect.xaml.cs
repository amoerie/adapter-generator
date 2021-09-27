using System.Windows;
using System.Windows.Media;
using AdapterGenerator.UserInterface.GeneratorModule.ViewModels;
using Microsoft.Win32;

namespace AdapterGenerator.UserInterface.GeneratorModule.Views {
  /// <summary>
  /// Interaction logic for SourceFileSelectView.xaml
  /// </summary>
  public partial class SourceFileSelect {
    public SourceFileSelect() {
      InitializeComponent();
    }

    private void SourceListView_Drop(object sender, DragEventArgs e) {
      SourceListView.ClearValue(BackgroundProperty);
      if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

      // Note that you can have more than one file.
      var paths = (string[]) e.Data.GetData(DataFormats.FileDrop);

      var viewModel = DataContext as SourceFileSelectViewModel;
      // Assuming you have one file that you care about, pass it off to whatever
      // handling code you have defined.
      foreach (var fileOrFolder in paths) {
        viewModel?.AddSourceFiles(fileOrFolder);
      }
    }


    private void SourceListView_DragOver(object sender, DragEventArgs e) {
      e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void SourceListView_DragEnter(object sender, DragEventArgs e) {
      SourceListView.Background = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#CC647687"));
    }

    private void SourceListView_DragLeave(object sender, DragEventArgs e) {
      SourceListView.ClearValue(BackgroundProperty);
    }

    private void AddSourceFiles_Click(object sender, RoutedEventArgs e) {
      var openFileDialog = new OpenFileDialog {
        Multiselect = true,
        Filter = "C# files (*.cs)|*.cs"
      };
      if (openFileDialog.ShowDialog() != true) return;

      var viewModel = DataContext as SourceFileSelectViewModel;
      foreach (var file in openFileDialog.FileNames) {
        viewModel?.AddSourceFiles(file);
      }
    }
  }
}