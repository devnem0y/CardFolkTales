using UnityEngine;

namespace UralHedgehog
{
    [System.Serializable] 
    public class AudioLibrary
    {
        [SerializeField] private string elementName;
    
        [SerializeField] private bool loop;
        public bool Loop { get => loop; set => loop = value; }
    }

    [System.Serializable]
    public class AudioLibSound : AudioLibrary
    {
        [SerializeField] private Sound track;
        public Sound Track => track;
    }

    [System.Serializable]
    public class AudioLibMusic : AudioLibrary
    {
        [SerializeField] private Music track;
        public Music Track => track;
    }

    [System.Serializable]
    public class AudioLibVoice : AudioLibrary
    {
        [SerializeField] private Voice track;
        public Voice Track => track;
    }
}
