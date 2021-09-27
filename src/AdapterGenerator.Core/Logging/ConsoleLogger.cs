using System;

namespace AdapterGenerator.Core.Logging {
  public class ConsoleLogger : ILogger {
    public void Info(string message) {
      Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} : {message}");
    }

    public void Error(string message) {
      Console.Error.WriteLine($"{DateTime.Now:HH:mm:ss.fff} : {message}");
    }
  }
}