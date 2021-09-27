namespace AdapterGenerator.Core.Logging {
  public interface ILogger {
    /// <summary>
    /// Writes a message to the standard log
    /// </summary>
    /// <param name="message">The message.</param>
    void Info(string message);

    /// <summary>
    /// Writes a message to the error log
    /// </summary>
    /// <param name="message">The message.</param>
    void Error(string message);
  }
}