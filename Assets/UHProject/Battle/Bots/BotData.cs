using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BotData
{
    [SerializeField] private List<CardData> _cardDeck;
    public List<CardData> CardDeck => _cardDeck;
}