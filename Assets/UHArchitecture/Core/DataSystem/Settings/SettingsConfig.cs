using UnityEngine;
using UralHedgehog;

namespace UralHedgehog
{
    [CreateAssetMenu(fileName = "SettingsConfig", menuName = "Data System/Settings", order = 2)]
    public class SettingsConfig : ScriptableObject
    {
        [SerializeField] private SettingsData _data;
        public SettingsData Data
        {
            get => _data;
            set => _data = value;
        }
    }
}
