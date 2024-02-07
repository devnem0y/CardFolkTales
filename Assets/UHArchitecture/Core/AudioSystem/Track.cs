using UnityEngine;

namespace UralHedgehog
{
    [System.Serializable]
    public class Track
    {
        [SerializeField] private string elementName;

        [SerializeField] private AudioClip clip;
        public AudioClip Clip { get => clip; set => clip = value; }
    
        [Range(0f,1f)]
        [SerializeField] private float volume;
        public float Volume { get => volume; set => volume = value; }
    }

    [System.Serializable]
    public class TrackSound : Track
    {
        [SerializeField] private Sound track;
        public Sound Track => track;
    }

    [System.Serializable]
    public class TrackMusic : Track
    {
        [SerializeField] private Music track;
        public Music Track => track;
    }

    [System.Serializable]
    public class TrackVoice : Track
    {
        [SerializeField] private Voice track;
        public Voice Track => track;
    }
}