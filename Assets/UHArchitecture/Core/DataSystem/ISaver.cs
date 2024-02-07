namespace UralHedgehog
{
    public interface ISaver
    {
        IData Data { get; }
        
        void Save();
    }
}