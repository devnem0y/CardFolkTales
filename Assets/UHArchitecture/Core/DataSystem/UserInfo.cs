using UralHedgehog;

[System.Serializable]
public struct UserInfo
{
    public string DateTime;
    public PlayerData PlayerData;

    public UserInfo(string dateTime, PlayerData playerData)
    {
        DateTime = dateTime;
        PlayerData = playerData;
    }
    
    public UserInfo(PlayerData playerData)
    {
        DateTime = "";
        PlayerData = playerData;
    }
}
