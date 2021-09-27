using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using AdapterGenerator.UserInterface.Events;
using AdapterGenerator.UserInterface.GeneratorModule.Handlers;
using AdapterGenerator.UserInterface.GeneratorModule.Messages;
using Prism.Commands;
using Prism.Mvvm;

namespace AdapterGenerator.UserInterface.GeneratorModule.ViewModels {
  public class TargetFileSelectViewModel : BindableBase {
    private readonly IFileOrFolderHandler _fileOrFolderHandler;

    public TargetFileSelectViewModel(IFileOrFolderHandler fileOrFolderHandler) {
      if (fileOrFolderHandler == null) throw new ArgumentNullException(nameof(fileOrFolderHandler));
      _fileOrFolderHandler = fileOrFolderHandler;
      RemoveTargetFileCommand = new DelegateCommand<FileInfo>(RemoveTargetFile);
    }

    public ObservableCollection<FileInfo> TargetFiles { get; set; } = new ObservableCollection<FileInfo>();
    public ICommand RemoveTargetFileCommand { get; set; }

    public void AddTargetFile(string path) {
      foreach (var fileInfo in _fileOrFolderHandler.Handle(path)) {
        if (TargetFiles.Any(file => file.FullName == fileInfo.FullName)) return;
        TargetFiles.Add(fileInfo);
        EventSystem.Publish(new TargetFileAddedMessage {TargetFile = fileInfo});
      }
    }

    private void RemoveTargetFile(FileInfo fileInfo) {
      TargetFiles.Remove(fileInfo);
      EventSystem.Publish(new TargetFileRemovedMessage {TargetFile = fileInfo});
    }
  }
}