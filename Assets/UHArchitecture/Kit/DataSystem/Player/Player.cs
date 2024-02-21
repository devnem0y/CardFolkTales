using System.Collections.Generic;
using System.Linq;
using UralHedgehog;
using UralHedgehog.UI;

public class Player : PlayerBase, IPlayer
{
    public string Name { get; }
    public int Level { get; }
    public int Exp { get; }

    public int CountCardDeck => _deck.Count;
    public int CountCardCollection => _collection.Count;

    public List<bool> TutorialsData { get; private set; }
    
    private readonly List<CardData> _deck;
    private readonly List<CardData> _collection;

    public Player(PlayerData data)
    {
        Data = data;

        Name = data.Name;
        Level = data.Level;
        Exp = data.Exp;

        _deck = new List<CardData>(data.Deck);
        _collection = new List<CardData>(data.Collection);
        
        var soft = new Soft(data.Soft);
        var hard = new Hard(data.Hard);
        CountersAdd(soft, hard);

        TutorialsData = data.TutorialsData;
    }
    
    public void AddCard(bool isDeck, Identity identity)
    {
        //TODO: Дорабатывать
        
        var card = new CardData(identity);
        
        if (isDeck)
        {
            _deck.Add(card);
        }
        else
        {
            _collection.Add(card);
        }
    }

    public void RemoveCard(bool isDeck, Identity identity)
    {
        //TODO: Дорабатывать
        
        var card = new CardData(identity);
        
        if (isDeck)
        {
            _deck.Remove(card);
        }
        else
        {
            _collection.Remove(card);
        }
    }

    public override void Save()
    {
        var soft = GetCounter<Soft>().Value;
        var hard = GetCounter<Hard>().Value;
        
        Data = new PlayerData(
            Name,
            Level,
            Exp,
            soft,
            hard,
            _deck,
            _collection,
            TutorialsData
            );
    }
    
    public T GetCounter<T>() where T : Counter
    {
        return _counters.Where(counter => counter.GetType() == typeof(T)).Cast<T>().FirstOrDefault();
    }
}