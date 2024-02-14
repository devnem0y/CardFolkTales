using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class PMainBottom : Widget
{
    [SerializeField] private Button _btnBattle;

    public override void Init()
    {
        _btnBattle.onClick.AddListener(() =>
        {
            //TODO: Для теста
            Game.Instance.ChangeState(GameState.PLAY);
            Hide();
        });
    }
}
