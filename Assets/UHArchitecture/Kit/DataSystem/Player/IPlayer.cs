namespace UralHedgehog
{
    public interface IPlayer : ICounters
    {
        public string Name { get; }
        public int Level { get; }
        public int Exp { get; }
        
        public int CountCardDeck { get; }
        public int CountCardCollection { get; }

        public void AddCard(bool isDeck, Identity identity);
        public void RemoveCard(bool isDeck, Identity identity);
    }
}