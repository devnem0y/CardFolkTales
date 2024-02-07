using System.Collections.Generic;
using UnityEngine.Audio;

namespace UralHedgehog
{
    public class AudioManager
    {
        private const string MASTER = "Master";
        private const string MUSIC = "Music";
        private const string SOUND = "Sound";
        private const string VOICE = "Voice";

        private readonly AudioMixer _audioMixer;

        public AudioMixerGroup AmgMusic => AMG(MUSIC);
        public AudioMixerGroup AmgSound => AMG(SOUND);
        public AudioMixerGroup AmgVoice => AMG(VOICE);
        
        public List<TrackMusic> Musics { get; }
        public List<TrackSound> Sounds { get; }
        public List<TrackVoice> Voices { get; }

        public AudioManager(AudioMixer audioMixer, AudioResources audioResources)
        {
            _audioMixer = audioMixer;

            Musics = audioResources.Musics;
            Sounds = audioResources.Sounds;
            Voices = audioResources.Voices;
        }

        private AudioMixerGroup AMG(string nameGroup)
        {
            switch (nameGroup)
            {
                case MUSIC:
                    return _audioMixer.FindMatchingGroups(MASTER)[1];
                case SOUND:
                    return _audioMixer.FindMatchingGroups(MASTER)[2];
                case VOICE:
                    return _audioMixer.FindMatchingGroups(MASTER)[3];
            }

            return null;
        }
    }
}