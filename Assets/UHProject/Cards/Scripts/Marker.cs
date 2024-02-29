using TMPro;
using UnityEngine;
using UralHedgehog;

public class Marker : MonoBehaviour
{
    [SerializeField] private MarkerType _type;
    public MarkerType Type => _type;
    [SerializeField] private TMP_Text _value;

    public void SetValue(string valueString)
    {
        _value.text = valueString;
    }
    
    public void SetValue(int value)
    {
        _value.text = $"{value}";
    }
}

public enum MarkerType
{
    HP = 0,
    DEFENSE = 1,
    ATTACK = 2,
    RAGE = 3,
    
    ARMOR_PENETRATION = 4,
}