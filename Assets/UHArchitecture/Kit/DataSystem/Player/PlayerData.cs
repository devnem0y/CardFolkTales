namespace UralHedgehog
{
    [System.Serializable]
    public struct PlayerData : IData
    {
        public string Name;
        public int Soft;

        public PlayerData(PlayerData data)
        {
            Name = data.Name;
            Soft = data.Soft;
        }
    
        public PlayerData(string name, int soft)
        {
            Name = name;
            Soft = soft;
        }
    }
}