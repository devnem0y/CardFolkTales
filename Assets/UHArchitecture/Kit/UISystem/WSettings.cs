using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class WSettings : Widget
{
    [SerializeField] private Button _btnClose;

    private ISettings _settings;

    protected override void Awake()
    {
        base.Awake();
        _btnClose.onClick.AddListener(() =>
        {
            //TODO: Для теста
            Game.Instance.ChangeState(Game.Instance.GameState == GameState.MAIN ? GameState.PLAY : GameState.MAIN);
            Hide();
        });
    }

    public override void Init(params object[] param)
    {
        _settings = (ISettings) param[0];
    }

    public override void Show()
    {
        base.Show();
        Open(() =>
        {
            //TODO: Можно вставить анимацию
            Debug.Log("Open callback");
        });
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