namespace AdapterGenerator.Core.Parsing {
  public interface IFileReader {
    /// <summary>
    /// Reads the contents of a file and returns it as a string
    /// </summary>
    /// <param name="absolutePath">The absolute path.</param>
    string ReadFile(string absolutePath);
  }
}