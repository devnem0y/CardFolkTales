using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace UralHedgehog
{
    public enum Language
    {
        ENGLISH = 0,
        RUSSIAN = 1,
        GERMAN = 2,
        FRENCH = 3,
        SPANISH = 4,
        ITALIAN = 5,
        CHINESE_SIMPLIFIED = 6,
    }

    public class LocalizationManager
    {
        private Dictionary<string, List<string>> _localization;

        public LocalizationConfig Config { get; }
        public Language Language { get; set; }

        public event Action Localize;

        public LocalizationManager(LocalizationConfig config)
        {
            Config = config;
            LoadLocalization();
        }

        private void LoadLocalization()
        {
            _localization = new Dictionary<string, List<string>>();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Config.LocalizationFile.text);

            foreach (XmlNode key in xmlDocument["Keys"].ChildNodes)
            {
                var keyStr = key.Attributes["Name"].Value;
                var values = new List<string>();
                
                foreach (XmlNode translate in key["Translates"].ChildNodes)
                {
                    values.Add(translate.InnerText);
                }

                _localization[keyStr] = values;
            }

            Debug.Log("Localization loading: <color=green>DONE</color>");
        }

        public string GetTranslate(string key)
        {
            return _localization.ContainsKey(key) ? 
                _localization[key][Language.GetHashCode()] : key;
        }

        public void OnLocalize()
        {
            Localize?.Invoke();
        }
    }
}