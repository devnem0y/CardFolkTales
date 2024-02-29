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

    public GameObject InstantiateCard(int id, Transform t, RectTransform wrapper)
    {
        var cardBase = GetDataCardBase(id);

        if (cardBase == null) return null;
        
        var item = Instantiate(cardBase.Type == CardType.UNIT ? _prefabCardUnit : _prefabCardBonus, t);
        var card = item.GetComponent<Card>();
        //var colorTeam = Game.Instance.GetColorTeam(team);
        var colorTeam = Color.red; //TODO: Просто пока забиваем цвет
        card.Init(cardBase, wrapper, null);
        return item;
    }
    
    public GameObject InstantiateCard(int id, Transform t, RectTransform wrapper, CommanderBase commander)
    {
        var cardBase = GetDataCardBase(id);

        if (cardBase == null) return null;
        
        var item = Instantiate(cardBase.Type == CardType.UNIT ? _prefabCardUnit : _prefabCardBonus, t);
        var card = item.GetComponent<Card>();
        //var colorTeam = Game.Instance.GetColorTeam(team);
        var colorTeam = Color.red; //TODO: Просто пока забиваем цвет
        card.Init(cardBase, wrapper, commander);
        return item;
    }
}
