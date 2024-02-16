using System.ComponentModel;

[System.Serializable]
public struct CardData
{
    //TODO: Пока только нужно хранить id. Если будет прокачка, то понадобятся еще разные параметры
    
    public Identity Id;
    

    public CardData(Identity id)
    {
        Id = id;
    }
}
