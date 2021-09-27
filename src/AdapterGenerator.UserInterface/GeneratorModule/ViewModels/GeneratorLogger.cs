using System;
using AdapterGenerator.Core.Logging;

namespace AdapterGenerator.UserInterface.GeneratorModule.ViewModels {
  public interface IGeneratorLogger {
    Action<string> Log { get; set; }
  }

  public class GeneratorLogger : ILogger, IGeneratorLogger {
    public void Info(string message) {
      Log($"{DateTime.Now:HH:mm:ss.fff} : {message}");
    }

    public void Error(string message) {
      Log($"{DateTime.Now:HH:mm:ss.fff} : {message}");
    }

    public Action<string> Log { get; set; }
  }
}