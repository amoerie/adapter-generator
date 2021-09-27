using System.Collections.Generic;

namespace AdapterGenerator.Tests.IntegrationTests.TestData.Sources {
  public class Speaker {
    public IEnumerable<Session> ListOfSessions { get; set; }
    public IList<Session> CollectionOfSessions { get; set; }
    public ICollection<Session> ArrayOfSessions { get; set; }
    public Session[] EnumerableOfSessions { get; set; }
  }

  public class Session {
    
  }
}