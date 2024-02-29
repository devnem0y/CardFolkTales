using System;
using System.Globalization;
using UnityEngine;

namespace UralHedgehog
{
    [CreateAssetMenu(fileName = "Loader", menuName = "Data System/Loader", order = 0)]
    public class Loader : ScriptableObject
    {
        //TODO: Config это установки по умолчанию (стартовые)
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private SettingsConfig _settingsConfig;
        
        //TODO: Mock это конфиги для редактора
        [SerializeField] private SettingsConfig _settingsMock;
        [SerializeField] private PlayerConfig _playerMock;

        [SerializeField] private bool _takingMock;
        
        public UserInfo UserInfo { get; private set; }
        public SettingsInfo SettingsInfo { get; private set; }
        public bool IsLoaded { get; private set; }
        
        public void Load()
        {
            const string keyUser = nameof(UserInfo);
            const string keySettings = nameof(SettingsInfo);
            
            var cultureInfo = new CultureInfo("en-us");

            #region User
            
            //Изначально берем из конфига по дефолту
            UserInfo = new UserInfo(_playerConfig.Data);

            var dateTimeLocal = "";
            var dateTimeCloud = "";
            var isLocal = false;
            var isCloud = false;
            PlayerData playerDataLocal = default;
            PlayerData playerDataCloud = default;

            if (PlayerPrefs.HasKey(keyUser)) //Если есть сохраненные данные в PlayerPrefs, достаем
            {
                var path = PlayerPrefs.GetString(keyUser);
                var userInfo = JsonUtility.FromJson<UserInfo>(path);
                dateTimeLocal = userInfo.DateTime;
                playerDataLocal = userInfo.PlayerData;
                isLocal = true;
                Debug.Log($"HAS-Local");
            }

            var myDataCloud = ""; //TODO: Строка из облака

            if (!string.IsNullOrEmpty(myDataCloud))
            {
                var userInfo = JsonUtility.FromJson<UserInfo>(myDataCloud);
                dateTimeCloud = userInfo.DateTime; 
                playerDataCloud = userInfo.PlayerData;
                isCloud = true;
            }

            if (isLocal)
            {
                if (isCloud) 
                {
                    var parseD1 = DateTime.Parse(dateTimeLocal, cultureInfo);
                    var parseD2 = DateTime.Parse(dateTimeCloud, cultureInfo);
            
                    var result = DateTime.Compare(parseD1, parseD2);
            
                    switch (result)
                    {
                        case < 0:
                            //TODO: Берем из облака
                            UserInfo = new UserInfo(playerDataCloud);
                            Debug.Log($"Cloud");
                            break;
                        case 0:
                            //TODO: Берем из PlayerPrefs
                            UserInfo = new UserInfo(playerDataLocal);
                            Debug.Log($"Local");
                            break;
                        default:
                            //TODO: Берем из PlayerPrefs и записываем в облако
                            UserInfo = new UserInfo(playerDataLocal);
                            //Game.Instance.SaveUser(true);
                            Debug.Log($"Local to cloud");
                            break;
                    }
                }
                else
                {
                    //TODO: Берем из PlayerPrefs и записываем в облако
                    UserInfo = new UserInfo(playerDataLocal);
                    //Game.Instance.SaveUser(true);
                    Debug.Log($"D - Local to cloud");
                }
                    
            }

            #endregion
            
            #region Settings

            if (PlayerPrefs.HasKey(keySettings)) //Если есть сохраненные данные, достаем
            {
                var path = PlayerPrefs.GetString(keySettings);
                SettingsInfo = JsonUtility.FromJson<SettingsInfo>(path);
            }
            else
            {
                //Иначе берем из конфига по дефолту
                SettingsInfo = new SettingsInfo(_settingsConfig.Data);
            }

            #endregion
            
#if UNITY_EDITOR // Если работаем в редакторе, достаем из моков при условии _takingMock = true

            if (_takingMock)
            {
                UserInfo = new UserInfo(_playerMock.Data);
                SettingsInfo = new SettingsInfo(_settingsMock.Data);
            }
#endif
            
            IsLoaded = true;
            Debug.Log("Data loading: <color=green>DONE</color>");
        }
    }
}