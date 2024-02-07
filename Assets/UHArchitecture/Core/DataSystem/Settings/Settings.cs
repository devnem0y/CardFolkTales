using System;
using UnityEngine;
using UnityEngine.Audio;

namespace UralHedgehog
{
    public class Settings : ISettings, ISaver
    {
        private const string MASTER = "Master";
        private const string MUSIC = "Music";
        private const string SOUND = "Sound";
        private const string VOICE = "Voice";
        
        private readonly AudioMixer _audioMixer;
        
        public IData Data { get; private set; }
        
        public float VolumeMaster { get; private set; }
        public float VolumeMusic { get; private set; }
        public float VolumeSound { get; private set; }
        public float VolumeVoice { get; private set; }
        public Language Language { get; private set; }

        public event Action OnChangeLanguage;

        public Settings(SettingsData data, AudioMixer audioMixer)
        {
            Data = data;
            _audioMixer = audioMixer;
            
            ChangeVolumeMaster(data.Master);
            ChangeVolumeMusic(data.Music);
            ChangeVolumeSound(data.Sound);
            ChangeVolumeVoice(data.Voice);
            ChangeLanguage(data.Language);
        }

        private void SetFloat(string nameGroup, float value)
        {
            _audioMixer.SetFloat(nameGroup, Mathf.Lerp(-80, 0, value));
        }
    
        public void ChangeVolumeMaster(float value)
        {
            VolumeMaster = value;
            SetFloat(MASTER, VolumeMaster);
        }

        public void ChangeVolumeMusic(float value)
        {
            VolumeMusic = value;
            SetFloat(MUSIC, VolumeMusic);
        }

        public void ChangeVolumeSound(float value)
        {
            VolumeSound = value;
            SetFloat(SOUND, VolumeSound);
        }

        public void ChangeVolumeVoice(float value)
        {
            VolumeVoice = value;
            SetFloat(VOICE, VolumeVoice);
        }

        public void ChangeLanguage(Language language)
        {
            Language = language;
            OnChangeLanguage?.Invoke();
        }

        public void Save()
        {
            Data = new SettingsData(VolumeMaster, VolumeMusic, VolumeSound, VolumeVoice, Language);
        }
    }
}
