using UnityEngine;
using UralHedgehog;

public class Player : PlayerBase, IPlayer
{
    public string Name { get; }

    public Player(PlayerData data)
    {
        Data = data;

        Name = data.Name;
        
        //TODO: Счетчики используются для ресурсов (в данном примере создали софт валюту и добавили к счетчикам)
        var soft = new Soft(data.Soft);
        var score = new Score(0);
        CountersAdd(soft, score);
    }

    public override void Save()
    {
        Data = new PlayerData(Name, GetCounter<Soft>().Value);
    }
}