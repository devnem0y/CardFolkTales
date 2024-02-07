using System;
using System.Globalization;
using UnityEngine;

namespace UralHedgehog
{
    [CreateAssetMenu(fileName = "Saver", menuName = "Data System/Saver", order = 1)]
    public class Saver : ScriptableObject
    {
        [SerializeField] private SettingsConfig _settingsMock;
        [SerializeField] private PlayerConfig _playerMock;
        
        /// <summary>
        /// В облако сохраняем только в последний момент, желательно в методе OnApplicationQuit().
        /// Все остальное сохранение происходит в PlayerPrefs. Можно вызывать чаще, но не злоупотреблять.
        /// </summary>
        public void Write(ISaver player, bool isCloud)
        {
            player.Save();
            
            var playerData = (PlayerData) player.Data;
            
#if UNITY_EDITOR
            _playerMock.Data = playerData;
#endif
            var cultureInfo = new CultureInfo("en-us");
            var dateTime = DateTime.Now.ToString(cultureInfo);
            var userInfo = new UserInfo(dateTime, playerData);
            var json = JsonUtility.ToJson(userInfo);
            
            PlayerPrefs.SetString(nameof(UserInfo), json); //TODO: Сохранение в PlayerPrefs всех данных игрока
            PlayerPrefs.Save();
            
            if (isCloud)
            {
                //TODO: Хранение в облако, реализовать
            }
            
            Debug.Log("Data save: <color=green>DONE</color>");
        }

        /// <summary>
        /// Сохранение настроек всегда происходит в PlayerPrefs.
        /// </summary>
        public void Write(ISaver settings)
        {
            settings.Save();
            
            var settingsData = (SettingsData) settings.Data;
            
#if UNITY_EDITOR
            _settingsMock.Data = settingsData;
#endif
            
            var settingsInfo = new SettingsInfo(settingsData);
            var json = JsonUtility.ToJson(settingsInfo);
            
            PlayerPrefs.SetString(nameof(SettingsInfo), json); //TODO: Сохранение в PlayerPrefs всех данных настроек игры
            PlayerPrefs.Save();
        }
    }
}