using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class PBattle : Widget
{
    [SerializeField] private Button _btnBackMain;

    public override void Init()
    {
        _btnBackMain.onClick.AddListener(() =>
        {
            Game.Instance.ChangeState(GameState.MAIN);
        });
    }
}
