using System.Collections.Generic;
using UnityEngine;

namespace UralHedgehog
{
    [System.Serializable]
    public struct PlayerData : IData
    {
        public string Name;
        public int Level;
        public int Exp;
        public int Soft;
        public int Hard;
        
        public List<CardData> Deck;
        public List<CardData> Collection;
        
        /*[HideInInspector] */public List<bool> TutorialsData;

        public PlayerData(PlayerData data)
        {
            Name = data.Name;
            Level = data.Level;
            Exp = data.Exp;
            Soft = data.Soft;
            Hard = data.Hard;
            Deck = data.Deck;
            Collection = data.Collection;
            TutorialsData = data.TutorialsData;
        }
    
        public PlayerData(string name, int level, int exp, int soft, int hard, List<CardData> deck, List<CardData> collection, List<bool> tutorialsData)
        {
            Name = name;
            Level = level;
            Exp = exp;
            Soft = soft;
            Hard = hard;
            Deck = deck;
            Collection = collection;
            TutorialsData = tutorialsData;
        }
    }
}