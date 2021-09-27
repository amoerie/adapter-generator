using System.IO;
using Prism.Events;

namespace AdapterGenerator.UserInterface.GeneratorModule.Messages {
  public class TargetFileAddedMessage : EventBase {
    public FileInfo TargetFile { get; set; }
  }
}