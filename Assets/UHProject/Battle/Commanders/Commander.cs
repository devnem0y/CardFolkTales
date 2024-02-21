using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
//using Event = UralHedgehog.Event;

public class Commander : CommanderBase
{
    [SerializeField] private Hand _handUnits;
    [SerializeField] private Hand _handBonuses;
    [SerializeField] private Battlefield _battlefield;
    [SerializeField] private RectTransform _wrapper;

    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _lblTurnPoints;
    [SerializeField] private TMP_Text _lblBonusesCount;
    [SerializeField] private TMP_Text _lblUnitsCount;
    [SerializeField] private GameObject _maskPlayer;
    [SerializeField] private GameObject _maskField;
    
    [SerializeField] private AudioComponent _audio;
    
    public Hand HandUnits => _handUnits;
    public Hand HandBonuses => _handBonuses;
    public Battlefield Battlefield => _battlefield;
    public RectTransform Wrapper => _wrapper;

    private Team _team;
    private CardStorage _cardStorage;

    public void Init(Team team, CardStorage cardStorage)
    {
        _team = team;
        //_avatar.color = Game.Instance.GetColorTeam(_team);
        _cardStorage = cardStorage;
    }

    public void Win()
    {
        //Dispatcher.Send(Event.ON_CHANGE_GAME_STATE, GameState.VICTORY);
        _maskPlayer.SetActive(true);
        _maskField.SetActive(true);
    }

    public void Lose()
    {
        //Dispatcher.Send(Event.ON_CHANGE_GAME_STATE, GameState.DEFEAT);
        _maskPlayer.SetActive(true);
        _maskField.SetActive(true);
    }

    public bool Clear(ControllerType controllerType)
    {
        _lblTurnPoints.text = string.Empty;
        _lblBonusesCount.text = string.Empty;
        _lblUnitsCount.text = string.Empty;
        
        foreach (Transform child in _handUnits.transform) Destroy(child.gameObject);
        foreach (Transform child in _handBonuses.transform) Destroy(child.gameObject);
        
        var cardCells = controllerType == ControllerType.PLAYER ? 
            _battlefield.PlayerFrontLine.Cells : 
            _battlefield.EnemyFrontLine.Cells;

        if (cardCells == null) return true;
        
        foreach (var cell in cardCells)
        {
            cell.Clear();
            if (cell.transform.childCount > 0)
            {
                Destroy(cell.transform.GetChild(0).gameObject);
            }
        }

        return true;
    }

    public void LabelTurnPointsUpdate(Counter turnPoints)
    {
        _lblTurnPoints.text = $"{turnPoints.Value}";
    }

    public void CardUse(ControllerType controllerType, Counter turnPoints, Deck deckUnits, Deck deckBonuses)
    {
        //_audio.Play(Sound.USE_UNIT_CARD);
        LabelTurnPointsUpdate(turnPoints);
        UpdateStateCards(controllerType, turnPoints, deckUnits, deckBonuses);
    }
    
    public void IssueCard(ControllerType controllerType, Counter turnPoints, Deck deckUnits, Deck deckBonuses)
    {
        var distUnits = DistributionOfCards(HandUnits, deckUnits);
        var distBonuses = DistributionOfCards(HandBonuses, deckBonuses);
        UpdateStateCards(controllerType, turnPoints, deckUnits, deckBonuses);
        
        if (controllerType == ControllerType.PLAYER)
        {
            //if (distUnits || distBonuses) _audio.Play(Sound.ISSUE_CARD);
        }
        else
        {
            HandUnits.CoverUp();
            HandBonuses.CoverUp();
        }
    }
    
    protected internal void UpdateStateCards(ControllerType controllerType, Counter turnPoints, Deck deckUnits, Deck deckBonuses)
    {
        _lblUnitsCount.text = deckUnits.Count.ToString();
        _lblBonusesCount.text = deckBonuses.Count.ToString();

        if (controllerType != ControllerType.PLAYER) return;

        for (var i = 0; i < _handUnits.CardCount; i++)
        {
            var child = _handUnits.transform.GetChild(i);
            child.GetComponent<Card>().StateUpdate(turnPoints.Value);
        }

        for (var i = 0; i < _handBonuses.CardCount; i++)
        {
            var child = _handBonuses.transform.GetChild(i);
            child.GetComponent<Card>().StateUpdate(turnPoints.Value);
        }
    }
    
    private void InstantiateCard(CardBase card)
    {
        var t = card.Type == CardType.UNIT ? _handUnits.transform : _handBonuses.transform;
        _cardStorage.InstantiateCard(card.Id, t, _wrapper, _team, this);
    }
    
    private bool DistributionOfCards(Hand hand, Deck deck)
    {
        var cardDistributionCount = 0;
        
        for (var i = hand.CardCount; i < 2; i++)
        {
            var card = deck.TakeCard();
            if (card == null) continue;
            InstantiateCard(card);
            cardDistributionCount++;
        }

        return cardDistributionCount > 0;
    }
}