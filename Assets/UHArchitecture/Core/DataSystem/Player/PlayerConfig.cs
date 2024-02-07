using UnityEngine;

namespace UralHedgehog
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Data System/Player", order = 3)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data
        {
            get => _data;
            set => _data = value;
        }
    }
}