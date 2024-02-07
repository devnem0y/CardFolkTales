using UralHedgehog;

[System.Serializable]
public struct SettingsInfo
{
    public SettingsData SettingsData;

    public SettingsInfo(SettingsData settingsData)
    {
        SettingsData = settingsData;
    }
}