namespace Flep
{
    using System.Collections.Generic;
    using Flep.Models;

    public interface IDataService
    {
        IEnumerable<Message> Get(Location point);

        void Add(Message m);
    }
}
