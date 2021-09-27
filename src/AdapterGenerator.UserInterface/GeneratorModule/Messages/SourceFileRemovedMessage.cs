using System.IO;

namespace AdapterGenerator.UserInterface.GeneratorModule.Messages {
  public class SourceFileRemovedMessage {
    public FileInfo SourceFile { get; set; }
  }
}