using System;
using UnityEngine;
using UralHedgehog;
using UralHedgehog.UI;

public class PLoseWin : Widget
{
    [SerializeField] private LoseGroup _lose;
    [SerializeField] private WinGroup _win;

    private bool _isWin;
    private int _rewardGems;
    private int _rewardCoins;

    public override void Init()
    {
        switch (Game.Instance.GameState)
        {
            case GameState.VICTORY:
                WinInit();
                break;
            case GameState.DEFEAT:
                LoseInit();
                break;
            case GameState.LOADING:
            case GameState.MAIN:
            case GameState.PLAY:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Show()
    {
        base.Show();
        
        if (_isWin) _win.PlayAnimStar();
    }

    private void WinInit()
    {
        _isWin = true;
        _win.gameObject.SetActive(true);
        
        _rewardGems = Game.Instance.Player.TurnPoints; // начисляем остатки очков хода
        _rewardCoins = 100; // будет браться из базы ревардов, пока заглушка
            
        _win.SetGames(_rewardGems);
        _win.SetCoins(_rewardCoins);
        
        _win.ClaimAddListener(() =>
        {
            //TODO: Просмотр рекламы и увеличение награды, пока сразу реакция
            ReactionRewardAdv();
        });
        
        _win.GetRewardAddListener(() =>
        {
            Game.Instance.Player.GetCounter<Hard>().Add(_rewardGems);
            Game.Instance.Player.GetCounter<Soft>().Add(_rewardCoins);
            
            BackHome();
        });
    }
    
    private void LoseInit()
    {
        _lose.gameObject.SetActive(true);
        
        _lose.RestartAddListener(() =>
        {
            //TODO: Через рекламу
            ReactionFullScreenAdv();
        });
        
        _lose.BackHomeAddListener(BackHome);
    }

    private void ReactionRewardAdv()
    {
        _rewardCoins *= _win.Multiplier;
        _win.SetCoins(_rewardCoins);
        _win.HideButtonClaim();
    }

    private void ReactionFullScreenAdv() // restart
    {
        Hide();
        Game.Instance.ChangeState(GameState.PLAY);
    }

    private void BackHome()
    {
        Hide();
        Game.Instance.ChangeState(GameState.MAIN);
    }
    
    public override void Hide()
    {
        Close(() =>
        {
            //TODO: Можно вставить анимацию
            Debug.Log("Close callback");
        });
        base.Hide();
    }
}
