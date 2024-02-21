using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UralHedgehog;

[CreateAssetMenu(fileName = "CardStorage", menuName = "Storage/Cards", order = 0)]
public class CardStorage : ScriptableObject
{
    [SerializeField] private GameObject _prefabCardUnit;
    [SerializeField] private GameObject _prefabCardBonus;
    [SerializeField] private List<CardBase> _cards;

    public CardBase GetDataCardBase(int id)
    {
        foreach (var card in _cards.Where(card => card.Id == id)) return card;

        Debug.Log($"<color=red>No such id = {id} found!</color>");
        return null;
    }

    public GameObject InstantiateCard(int id, Transform t, RectTransform wrapper, Team team)
    {
        var cardBase = GetDataCardBase(id);

        if (cardBase == null) return null;
        
        var item = Instantiate(cardBase.Type == CardType.UNIT ? _prefabCardUnit : _prefabCardBonus, t);
        var card = item.GetComponent<Card>();
        //var colorTeam = Game.Instance.GetColorTeam(team);
        //card.Init(cardBase, colorTeam, wrapper, null);
        return item;
    }
    
    public GameObject InstantiateCard(int id, Transform t, RectTransform wrapper, Team team, CommanderBase commander)
    {
        var cardBase = GetDataCardBase(id);

        if (cardBase == null) return null;
        
        var item = Instantiate(cardBase.Type == CardType.UNIT ? _prefabCardUnit : _prefabCardBonus, t);
        var card = item.GetComponent<Card>();
        //var colorTeam = Game.Instance.GetColorTeam(team);
        //card.Init(cardBase, colorTeam, wrapper, commander);
        return item;
    }
}
