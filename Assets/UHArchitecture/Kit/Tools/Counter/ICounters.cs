using System;

namespace UralHedgehog
{
    public interface ICounters
    {
        T GetCounter<T>() where T : Counter
        {
            throw new InvalidOperationException();
        }
    }
}