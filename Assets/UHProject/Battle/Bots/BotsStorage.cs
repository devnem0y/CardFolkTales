using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BotsStorage", menuName = "Storage/Bots", order = 1)]
public class BotsStorage : ScriptableObject
{
    [SerializeField] private List<BotData> _bots;
    public List<BotData> Bots => _bots;
}