namespace AdapterGenerator.UserInterface.GeneratorModule.Views {
  /// <summary>
  /// Interaction logic for GeneratorLog.xaml
  /// </summary>
  public partial class GeneratorLog {
    public GeneratorLog() {
      InitializeComponent();
    }

    private void LogBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
      LogScrollViewer.UpdateLayout();
      LogScrollViewer.ScrollToVerticalOffset(LogScrollViewer.ScrollableHeight);
    }
  }
}