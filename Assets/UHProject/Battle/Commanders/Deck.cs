using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    private readonly List<CardBase> _cards;

    public int Count => _cards.Count;

    public Deck(CardStorage cardStorage, IEnumerable<CardData> _cardData, CardType type)
    {
        _cards = new List<CardBase>();

        foreach (var card in _cardData.Select(data => 
                     cardStorage.GetDataCardBase(data.Id.GetHashCode())).
                     Where(card => card.Type == type)) _cards.Add(card);
        
        Shuffle(_cards);
    }

    /// <summary>
    /// Взять верхнюю карту из колоды
    /// </summary>
    public CardBase TakeCard()
    {
        if (Count <= 0) return null;

        var card = new CardBase(_cards[0]);
        _cards.RemoveAt(0);
        return card;
    }
    
    private static void Shuffle<T>(IList<T> list)
    {
        var rnd = new Random();

        for (var i = list.Count - 1; i >= 1; i--)
        {
            var j = rnd.Next(i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }
}