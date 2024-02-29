using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UralHedgehog.UI;

namespace UralHedgehog
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Loader _loader;
        [SerializeField] private Saver _saver;
        
        [SerializeField] private LocalizationConfig _localizationConfig;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioResources _audioResources;
        
        [SerializeField] private ScreenTransition _screenTransition;
        
        public LocalizationManager LocalizationManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public GameState GameState { get; private set; }

        public ScreenTransition ScreenTransition => _screenTransition;

        public event Action Loading;
        public event Action Launch;
        public event Action Begin;

        private bool _init;
        
        private Settings _settings;
        protected Player _player;

        protected void Run()
        {
            ChangeState(GameState.LOADING);
            StartCoroutine(AlternateСall());
        }
        
        private IEnumerator AlternateСall()
        {
            _loader.Load();
            yield return new WaitUntil(() => _loader.IsLoaded);
            Loading?.Invoke();
            yield return new WaitForSeconds(0.01f);
            Initialization();
            yield return new WaitUntil(() => _init);
            Launch?.Invoke();
            yield return new WaitForSeconds(0.01f);
            Begin?.Invoke();
            OnBegin();
        }

        /// <summary>
        /// Base не удалять! Все что нужно писать после.
        /// </summary>
        protected virtual void Initialization()
        {
            _settings = new Settings(_loader.SettingsInfo.SettingsData, _audioMixer);
            LocalizationManager = new LocalizationManager(_localizationConfig) { Language = _settings.Language };
            _settings.OnChangeLanguage += OnLocalize;
            AudioManager = new AudioManager(_audioMixer, _audioResources);
            Debug.Log($"dsgsdgsg {_loader.UserInfo.PlayerData.Name}");
            _player = new Player(_loader.UserInfo.PlayerData);
            
            _init = true;
        }
        
        protected virtual void OnBegin()
        {
            ChangeState(GameState.MAIN);
        }

        public void SaveSettings()
        {
            _saver.Write(_settings);
        }
        
        public void SaveUser(bool isCloud = false)
        {
            _saver.Write(_player, isCloud);
        }

        public virtual void ChangeState(GameState state)
        {
            GameState = state;
        }

        /// <summary>
        /// Base не удалять! Все что нужно писать после.
        /// </summary>
        protected virtual void OnDestroy()
        {
            _settings.OnChangeLanguage -= OnLocalize;
        }

        /// <summary>
        /// Поднимает виджет настроек
        /// </summary>
        public void OpenViewSettings()
        {
            var wSettingsData = new Data(nameof(WSettings), _settings);
            UIDispatcher.Send(EventUI.SHOW_WIDGET, wSettingsData);
        }

        private void OnLocalize()
        {
            LocalizationManager.OnLocalize();
        }
        
        private void OnApplicationQuit()
        {
            SaveSettings();
            SaveUser(true);
        }
    }
}