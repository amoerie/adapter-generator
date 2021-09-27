using System.IO;

namespace AdapterGenerator.UserInterface.GeneratorModule.Messages {
  public class TargetFileRemovedMessage {
    public FileInfo TargetFile { get; set; }
  }
}