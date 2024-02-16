using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CardBase
{
    [SerializeField] private string _name;
    public string Name => _name;
    [SerializeField] private Identity _identity;
    public int Id => _identity.GetHashCode();
    [Space(3)]
    [SerializeField, ShowAssetPreview] private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField, Multiline(5)] private string _description;
    public string Description => _description;
    [Space(3)]
    [SerializeField] private int _turnPoints;
    public int TurnPoints => _turnPoints;
    [Space(3)]
    [SerializeField] private CardType _type;
    public CardType Type => _type;

    [Space(5)]
    
    [SerializeField, HideIf(nameof(_type), CardType.UNIT), AllowNesting] 
    private BonusType _bonusType;
    public BonusType BonusType => _bonusType;
    [SerializeField, HideIf(nameof(_type), CardType.UNIT), AllowNesting]
    private int _magnitude;
    public int Magnitude => _magnitude;
    
    [Space(5)]
    
    [SerializeField, HideIf(nameof(_type), CardType.BONUS), AllowNesting] 
    private UnitType _unitType;
    public UnitType UnitType => _unitType;
    [SerializeField, HideIf(nameof(_type), CardType.BONUS), AllowNesting]
    private int _hp;
    public int Hp => _hp;
    [SerializeField, HideIf(nameof(_type), CardType.BONUS), AllowNesting]
    private int _attack;
    public int Attack => _attack;
    [SerializeField, HideIf(nameof(_type), CardType.BONUS), AllowNesting]
    private int _defense;
    public int Defense => _defense;

    public CardBase()
    {
        
    }
    
    public CardBase(CardBase cardBase)
    {
        _name = cardBase.Name;
        _identity = (Identity)cardBase.Id;
        _type = cardBase.Type;
        _icon = cardBase.Icon;
        _description = cardBase.Description;
        _turnPoints = cardBase.TurnPoints;
        _unitType = cardBase.UnitType;
        _hp = cardBase.Hp;
        _attack = cardBase.Attack;
        _defense = cardBase.Defense;
        _bonusType = cardBase.BonusType;
        _magnitude = cardBase.Magnitude;
    }
}