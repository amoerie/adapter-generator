using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using AdapterGenerator.Core;
using AdapterGenerator.UserInterface.Events;
using AdapterGenerator.UserInterface.GeneratorModule.Messages;
using Prism.Commands;
using Prism.Mvvm;

namespace AdapterGenerator.UserInterface.GeneratorModule.ViewModels {
  public class GeneratorViewModel : BindableBase {
    private readonly IAdapterGeneratorService _adapterGeneratorService;
    private readonly List<FileInfo> _sourceFiles = new List<FileInfo>();
    private readonly List<FileInfo> _targetFiles = new List<FileInfo>();
    private int _amountSourceFiles;
    private int _amountTargetFiles;

    public int AmountSourceFiles {
      get { return _amountSourceFiles; }
      set { SetProperty(ref _amountSourceFiles, value); }
    }

    public int AmountTargetFiles {
      get { return _amountTargetFiles; }
      set { SetProperty(ref _amountTargetFiles, value); }
    }

    public bool LoggerExpanderToggle { get; set; }
    public bool SourceExpanderToggle { get; set; } = true;
    public bool TargetExpanderToggle { get; set; } = true;
    public ICommand GenerateCommand { get; set; }

    public GeneratorViewModel(IAdapterGeneratorService adapterGeneratorService) {
      if (adapterGeneratorService == null) throw new ArgumentNullException(nameof(adapterGeneratorService));
      _adapterGeneratorService = adapterGeneratorService;

      GenerateCommand = new DelegateCommand(Generate);

      EventSystem.Subscribe<SourceFileAddedMessage>(SourceFileAdded);
      EventSystem.Subscribe<SourceFileRemovedMessage>(SourceFileRemoved);
      EventSystem.Subscribe<TargetFileAddedMessage>(TargetFileAdded);
      EventSystem.Subscribe<TargetFileRemovedMessage>(TargetFileRemoved);
    }


    private void SourceFileAdded(SourceFileAddedMessage message) {
      _sourceFiles.Add(message.SourceFile);
      AmountSourceFiles++;
    }

    private void SourceFileRemoved(SourceFileRemovedMessage message) {
      _sourceFiles.Remove(message.SourceFile);
      AmountSourceFiles--;
    }

    private void TargetFileAdded(TargetFileAddedMessage message) {
      _targetFiles.Add(message.TargetFile);
      AmountTargetFiles++;
    }

    private void TargetFileRemoved(TargetFileRemovedMessage message) {
      _targetFiles.Remove(message.TargetFile);
      AmountTargetFiles--;
    }


    private void Generate() {
      SetLoggerAsOnlyView();
      var outputDirectory = Path.Combine(AssemblyDirectory, "output");
      CleanOrCreateDirectory(outputDirectory);
      StartGenerateAdapters(outputDirectory);
    }

    private void StartGenerateAdapters(string outputDirectory) {
      Task.Factory.StartNew(() => {
#if !DEBUG
        try {
#endif
        _adapterGeneratorService.GenerateAdapters(
          _sourceFiles.ToImmutableList(),
          _targetFiles.ToImmutableList(),
          new DirectoryInfo(outputDirectory));
        OpenDirectory(outputDirectory);
#if !DEBUG
        }
        catch (Exception ex) {
          EventSystem.Publish(new GenerateErrorMessage {Exception = ex});
        }
#endif
      });
    }


    private void SetLoggerAsOnlyView() {
      SourceExpanderToggle = false;
      OnPropertyChanged(() => SourceExpanderToggle);
      TargetExpanderToggle = false;
      OnPropertyChanged(() => TargetExpanderToggle);
      LoggerExpanderToggle = true;
      OnPropertyChanged(() => LoggerExpanderToggle);
    }

    private static void CleanOrCreateDirectory(string directory) {
      if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
      else Array.ForEach(Directory.GetFiles(directory), File.Delete);
    }

    private static void OpenDirectory(string directory) {
      System.Diagnostics.Process.Start("explorer.exe", directory);
    }

    private static string AssemblyDirectory {
      get {
        var codeBase = Assembly.GetExecutingAssembly().CodeBase;
        var uri = new UriBuilder(codeBase);
        var path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
      }
    }
  }
}