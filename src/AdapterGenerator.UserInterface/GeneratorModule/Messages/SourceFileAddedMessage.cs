using System.IO;
using Prism.Events;

namespace AdapterGenerator.UserInterface.GeneratorModule.Messages {
  public class SourceFileAddedMessage : EventBase {
    public FileInfo SourceFile { get; set; }
  }
}