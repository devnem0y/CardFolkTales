using UnityEngine;

namespace UralHedgehog
{
    [System.Serializable]
    public struct SettingsData : IData
    {
        [Header("Громкость")]
        [Space(3)]
        
        [Range(0f, 1f)] public float Master;
        [Range(0f, 1f)] public float Music;
        [Range(0f, 1f)] public float Sound;
        [Range(0f, 1f)] public float Voice;

        [Space(3)]
        [Header("Язык")]
        [Space(3)]
        
        public Language Language;

        public SettingsData(SettingsData data)
        {
            Master = data.Master;
            Music = data.Music;
            Sound = data.Sound;
            Voice = data.Voice;
            Language = data.Language;
        }
        
        public SettingsData(float master, float music, float sound, float voice, Language language)
        {
            Master = master;
            Music = music;
            Sound = sound;
            Voice = voice;
            Language = language;
        }
    }
}
