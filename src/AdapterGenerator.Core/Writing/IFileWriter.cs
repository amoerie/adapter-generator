namespace AdapterGenerator.Core.Writing {
  public interface IFileWriter {
    void WriteFile(string path, string contents);
  }
}