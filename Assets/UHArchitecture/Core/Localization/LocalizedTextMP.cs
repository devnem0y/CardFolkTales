using TMPro;
using UnityEngine;

namespace UralHedgehog
{
    public class LocalizedTextMP : MonoBehaviour
    {
        [SerializeField] private string _key;
        
        public string Key
        {
            get => _key;
            set => _key = value;
        }
        
        public string Param { get; set; }
        public string Prefix { get; set; }

        private Bootstrap _bootstrap;
        private TMP_Text _label;
        private LocalizationManager _localizationManager;

        private void Awake()
        {
            _bootstrap = FindObjectOfType<Bootstrap>();
            _label = GetComponent<TMP_Text>();
        }

        private void OnDestroy()
        {
            _localizationManager.Localize -= Localize;
        }

        private void Start()
        {
            _localizationManager = _bootstrap.LocalizationManager;
            _localizationManager.Localize += Localize;
            Localize();
        }

        private void Localize()
        {
            if (_localizationManager == null) return;
            
            switch (_localizationManager.Language)
            {
                case Language.CHINESE_SIMPLIFIED:
                    _label.font = _localizationManager.Config.ChineseTMP;
                    break;
                case Language.RUSSIAN:
                    _label.font = _localizationManager.Config.BasicTMP;
                    break;
                case Language.ENGLISH:
                case Language.GERMAN:
                case Language.FRENCH:
                case Language.SPANISH:
                case Language.ITALIAN:
                    _label.font = _localizationManager.Config.ExtraTMP;
                    break;
            }

            var str = _localizationManager.GetTranslate(_key);
            if (string.IsNullOrEmpty(Prefix))
            {
                _label.text = string.IsNullOrEmpty(Param) ? str : $"{str}\n{Param}";
            }
            else
            {
                _label.text = string.IsNullOrEmpty(Param) ? $"{Prefix}{str}" : $"{Prefix}{str}\n{Param}";
            }
        }
    }
}