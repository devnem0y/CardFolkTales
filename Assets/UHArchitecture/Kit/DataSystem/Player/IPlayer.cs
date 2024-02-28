namespace UralHedgehog
{
    public interface IPlayer : ICounters
    {
        public string Name { get; }
        public int Level { get; }
        public int Exp { get; }
        
        public int CountCardDeck { get; }
        public int CountCardCollection { get; }

        public void AddCard(Identity identity, Pocket pocket);
        public void RemoveCard(Identity identity, Pocket pocket);
    }
}