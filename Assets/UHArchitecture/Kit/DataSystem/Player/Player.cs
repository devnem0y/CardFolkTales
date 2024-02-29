using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UralHedgehog;

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
    
    private CardStorage _cardStorage;
    private Coroutine _routineBattle;

    public Player(PlayerData data)
    {
        Data = new PlayerData(data);

        Name = data.Name;
        Debug.Log($"{data.Name}");
        Level = data.Level;
        Exp = data.Exp;

        _deck = new List<CardData>(data.Deck);

        foreach (var card in _deck)
        {
            Debug.Log(card.Id);
        }
        
        _collection = new List<CardData>(data.Collection);
        
        var soft = new Soft(data.Soft);
        var hard = new Hard(data.Hard);
        CountersAdd(soft, hard);
        
        TutorialsData = data.TutorialsData;
        
        _controllerType = ControllerType.PLAYER;
    }
    
    public override void Preparation()
    {
        base.Preparation();
        _deckUnits = new Deck(_cardStorage, _deck, CardType.UNIT);
        _deckBonuses = new Deck(_cardStorage, _deck, CardType.BONUS);
        _maxTurnPoints = Game.Instance.TutorialHandler.IsCompleted(0) ? 7 : 1;
        _turnPoints = _maxTurnPoints;
        _commander.Init(_cardStorage);
    }
    
    public override void Turn()
    {
        if (_routineBattle != null) return;

        _routineBattle = _commander.StartCoroutine(Battle(_commander.Battlefield.PlayerFrontLine.Cells,
            _commander.Battlefield.EnemyFrontLine.Cells, 0.25f, 1.3f,
            () =>
            {
                InvokerTurnDone();
                if (!Game.Instance.TutorialHandler.IsCompleted(0))
                {
                    if (Game.Instance.TutorialHandler.IsCompletedTutorialStep(0, 0))
                    {
                        _maxTurnPoints = 7;
                        _turnPoints = _maxTurnPoints;
                    }
                }
                AddTurnPoints();
                UpdateStateCards();
                if (_routineBattle != null) _commander.StopCoroutine(_routineBattle);
                _routineBattle = null;
            }));
    }
    
    public override void EndBattle()
    {
        base.EndBattle();
        _routineBattle = null;
    }

    public override void Win()
    {
        EndBattle();
        _commander.Win();
    }

    public override void Lose()
    {
        EndBattle();
        _commander.Lose();
    }

    public void SetCardStorage(CardStorage cardStorage)
    {
        _cardStorage = cardStorage;
    }
    
    public void AddCard(Identity identity, Pocket pocket)
    {
        var card = new CardData(identity);
        
        if (pocket == Pocket.DECK)
        {
            _deck.Add(card);
            
            if (CountCardDeck == 1) Dispatcher.Send(EventD.ON_DECK_UPDATE);
        }
        else
        {
            _collection.Add(card);
            
            Dispatcher.Send(EventD.ON_COLLECTION_UPDATE);
        }
    }

    public void RemoveCard(Identity identity, Pocket pocket)
    {
        var isDeck = pocket == Pocket.DECK;
        var list = isDeck ? _deck : _collection;
        var index = 0;

        for (var i = 0; i < list.Count; i++)
        {
            if (list[i].Id != identity) continue;
            index = i;
            break;
        }
            
        list.RemoveAt(index);
        
        Dispatcher.Send(isDeck ? EventD.ON_DECK_UPDATE : EventD.ON_COLLECTION_UPDATE);
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
    
    protected internal void TutorialsCheckData(List<Tutorial> _tutorials)
    {
        //TODO: Переделать реализацию, возможно вообще будет не нужно
        /*_data.TutorialsData.Clear();
        foreach (var tutorial in _tutorials) _data.TutorialsData.Add(tutorial.IsCompleted);*/
    }
}