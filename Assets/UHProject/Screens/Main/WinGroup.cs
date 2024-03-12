using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinGroup : MonoBehaviour
{
    [SerializeField] private Button _btnClaim;
    [SerializeField] private Button _btnGetReward;
    [SerializeField] private TMP_Text _lblGems;
    [SerializeField] private TMP_Text _lblCoins;
    [SerializeField] private TMP_Text _lblMultiplier;
    [SerializeField] private StarWin _star;
    [SerializeField] private int _multiplier;

    public int Multiplier => _multiplier;

    private void Start()
    {
        if (_multiplier > 1)
        {
            _lblMultiplier.text = $"X{_multiplier}";
            _btnClaim.gameObject.SetActive(true);
        }
    }

    public void ClaimAddListener(UnityAction action)
    {
        _btnClaim.onClick.AddListener(action);
    }
    
    public void GetRewardAddListener(UnityAction action)
    {
        _btnGetReward.onClick.AddListener(action);
    }

    public void SetGames(int value)
    {
        _lblGems.text = $"{value}";
    }
    
    public void SetCoins(int value)
    {
        _lblCoins.text = $"{value}";
    }

    public void PlayAnimStar()
    {
        _star.Begin();
    }

    public void HideButtonClaim()
    {
        _btnClaim.gameObject.SetActive(false);
    }
}