using System.Collections.Generic;
using System.Linq;

namespace UralHedgehog
{
    public class PlayerBase : ISaver, ICounters
    {
        public IData Data { get; protected set; }

        private readonly List<Counter> _counters = new();
        
        public virtual void Save() { }

        public T GetCounter<T>() where T : Counter
        {
            return _counters.Where(counter => counter.GetType() == typeof(T)).Cast<T>().FirstOrDefault();
        }
        
        protected void CountersAdd(params Counter[] counters)
        {
            foreach (var counter in counters) _counters.Add(counter);
        }
    }
}