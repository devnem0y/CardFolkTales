using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class WSettings : Widget
{
    [SerializeField] private Button _btnClose;
    [SerializeField] private Button _btnBackMain;

    private ISettings _settings;

    protected override void Awake()
    {
        base.Awake();
        
        _btnClose.onClick.AddListener(Hide);
        
        _btnBackMain.onClick.AddListener(() =>
        {
            Hide();
            Game.Instance.ChangeState(GameState.MAIN);
        });
    }

    public override void Init(params object[] param)
    {
        _settings = (ISettings) param[0];
        
        _btnBackMain.gameObject.SetActive(Game.Instance.GameState == GameState.PLAY);
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