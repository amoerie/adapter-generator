using System;
using Prism.Events;

namespace AdapterGenerator.UserInterface.GeneratorModule.Messages {
  public class GenerateErrorMessage : EventBase {
    public Exception Exception { get; set; }
  }
}