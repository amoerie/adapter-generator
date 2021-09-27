using System.Collections.Generic;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Targets {
  public class Speaker {
    public IEnumerable<Session> EnumerableOfSessions { get; set; }
    public IList<Session> ListOfSessions { get; set; }
    public ICollection<Session> CollectionOfSessions { get; set; }
    public Session[] ArrayOfSessions { get; set; }
  }

  public class Session {
    
  }
}