namespace Flep.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Flep.Models;

    public class MockDataService : IDataService
    {
        private List<Message> messages;

        public MockDataService()
        {
            this.messages = new List<Message>();
        }

        public IEnumerable<Message> Get(Location point)
        {
            return this.messages;
        }

        public void Add(Message msg)
        {
            this.messages.Add(msg);
            this.messages = this.messages
                .OrderByDescending(m => m.DateSubmitted)
                .ToList();
        }
    }
}
