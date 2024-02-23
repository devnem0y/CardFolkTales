using System.Collections.Generic;

namespace UralHedgehog
{
    public class PlayerBase : Controller, ISaver, ICounters
    {
        public IData Data { get; protected set; }

        protected readonly List<Counter> _counters = new();
        
        public virtual void Save() { }
        
        protected void CountersAdd(params Counter[] counters)
        {
            foreach (var counter in counters) _counters.Add(counter);
        }
    }
}