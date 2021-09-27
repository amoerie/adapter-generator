using System.Collections.Generic;
using System.IO;

namespace AdapterGenerator.UserInterface.GeneratorModule.Handlers {
  public interface IFileOrFolderHandler {
    IEnumerable<FileInfo> Handle(string path);
  }

  public class FileOrFolderHandler : IFileOrFolderHandler {
    public IEnumerable<FileInfo> Handle(string path) {
      var attributes = File.GetAttributes(path);
      if (attributes.HasFlag(FileAttributes.Directory)) {
        foreach (var filePath in Directory.GetFiles(path, "*.cs")) {
          yield return new FileInfo(filePath);
        }
      }
      else if (Path.GetExtension(path).ToLower() == ".cs") {
        yield return new FileInfo(path);
      }
    }
  }
}