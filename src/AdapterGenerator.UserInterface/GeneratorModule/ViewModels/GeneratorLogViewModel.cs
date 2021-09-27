using System;
using System.Collections.ObjectModel;
using System.Windows;
using AdapterGenerator.UserInterface.Events;
using AdapterGenerator.UserInterface.GeneratorModule.Messages;
using Prism.Mvvm;

namespace AdapterGenerator.UserInterface.GeneratorModule.ViewModels {
  public class GeneratorLogViewModel : BindableBase {
    public IGeneratorLogger GeneratorLogger { get; set; }
    public ObservableCollection<string> LogMessages { get; set; } = new ObservableCollection<string>();

    public GeneratorLogViewModel(IGeneratorLogger generatorLogger) {
      if (generatorLogger == null) throw new ArgumentNullException(nameof(generatorLogger));

      GeneratorLogger = generatorLogger;
      GeneratorLogger.Log = s => {
        Application.Current.Dispatcher.Invoke(() => {
          LogMessages.Add(s);
          OnPropertyChanged(() => LogMessages);
        });
      };

      EventSystem.Subscribe<GenerateErrorMessage>(LogError);
    }

    private void LogError(GenerateErrorMessage message) {
      Application.Current.Dispatcher.Invoke(() => {
        LogMessages.Add(new string('#', message.Exception.Message.Length));
        LogMessages.Add(message.Exception.Message);
        OnPropertyChanged(() => LogMessages);
      });
    }
  }
}