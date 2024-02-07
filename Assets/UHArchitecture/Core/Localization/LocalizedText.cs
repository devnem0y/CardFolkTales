using System;
using UnityEngine;
using UnityEngine.UI;

namespace UralHedgehog
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;

        private Bootstrap _bootstrap;
        private Text _label;
        private LocalizationManager _localizationManager;

        private void Awake()
        {
            _bootstrap = FindObjectOfType<Bootstrap>();
            _label = GetComponent<Text>();
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
            switch (_localizationManager.Language)
            {
                case Language.CHINESE_SIMPLIFIED:
                    _label.font = _localizationManager.Config.Chinese;
                    break;
                case Language.RUSSIAN:
                    _label.font = _localizationManager.Config.Basic;
                    break;
                case Language.ENGLISH:
                case Language.GERMAN:
                case Language.FRENCH:
                case Language.SPANISH:
                case Language.ITALIAN:
                    _label.font = _localizationManager.Config.Extra;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _label.text = _localizationManager.GetTranslate(_key);
        }
    }
}