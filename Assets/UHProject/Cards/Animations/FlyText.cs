using System;
using TMPro;
using UnityEngine;
using UralHedgehog;

public class FlyText : MonoBehaviour
{
    [SerializeField] private TMP_Text _str;
    
    [SerializeField] private Color _colorRed;
    [SerializeField] private Color _colorGreen;
    [SerializeField] private Color _colorWhite;
    [SerializeField] private Color _colorGray;

    public void Set(string str, FlyTextColor textColor)
    {
        _str.color = textColor switch
        {
            FlyTextColor.RED => _colorRed,
            FlyTextColor.GREEN => _colorGreen,
            FlyTextColor.WHITE => _colorWhite,
            FlyTextColor.GRAY => _colorGray,
            _ => throw new ArgumentOutOfRangeException(nameof(textColor), textColor, null)
        };
        
        _str.text = Game.Instance.LocalizationManager.GetTranslate(str);
        _str.GetComponent<LocalizedTextMP>().Key = str;
    }
    
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}

public enum FlyTextColor
{
    RED,
    GREEN,
    WHITE,
    GRAY,
}
