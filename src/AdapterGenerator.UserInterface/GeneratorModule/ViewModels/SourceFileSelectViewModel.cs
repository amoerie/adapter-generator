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
  public class SourceFileSelectViewModel : BindableBase {
    private readonly IFileOrFolderHandler _fileOrFolderHandler;

    public SourceFileSelectViewModel(IFileOrFolderHandler fileOrFolderHandler) {
      if (fileOrFolderHandler == null) throw new ArgumentNullException(nameof(fileOrFolderHandler));
      _fileOrFolderHandler = fileOrFolderHandler;
      RemoveSourceFileCommand = new DelegateCommand<FileInfo>(RemoveSourceFile);
    }

    public ObservableCollection<FileInfo> SourceFiles { get; set; } = new ObservableCollection<FileInfo>();
    public ICommand RemoveSourceFileCommand { get; set; }

    public void AddSourceFiles(string path) {
      foreach (var fileInfo in _fileOrFolderHandler.Handle(path)) {
        if (SourceFiles.Any(file => file.FullName == fileInfo.FullName)) return;
        SourceFiles.Add(fileInfo);
        EventSystem.Publish(new SourceFileAddedMessage {SourceFile = fileInfo});
      }
    }

    private void RemoveSourceFile(FileInfo fileInfo) {
      SourceFiles.Remove(fileInfo);
      EventSystem.Publish(new SourceFileRemovedMessage {SourceFile = fileInfo});
    }
  }
}