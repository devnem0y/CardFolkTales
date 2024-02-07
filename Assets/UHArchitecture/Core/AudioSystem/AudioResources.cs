using System.Collections.Generic;
using UnityEngine;

namespace UralHedgehog
{
    [CreateAssetMenu(fileName = "AudioResources", menuName = "AudioResources")]
    public class AudioResources : ScriptableObject
    {
        [SerializeField] private List<TrackMusic> _musics;
        public List<TrackMusic> Musics => _musics;
        [SerializeField] private List<TrackSound> _sounds;
        public List<TrackSound> Sounds => _sounds;
        [SerializeField] private List<TrackVoice> _voices;
        public List<TrackVoice> Voices => _voices;
    }
}
