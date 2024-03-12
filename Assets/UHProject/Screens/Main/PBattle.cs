using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class PBattle : Widget
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Commander _player;
    [SerializeField] private Commander _enemy;
    [SerializeField] private Button _btnTurn;
    /*[SerializeField] private Image _btnTurnIcon;
    [SerializeField] private Color _btnTurnIconC1;
    [SerializeField] private Color _btnTurnIconC2;*/
    //[SerializeField] private TMP_Text _lblBtnTurn;
    [SerializeField] private GameObject _maskPlayer;
    [SerializeField] private GameObject _maskField;
    //[SerializeField] private GameObject _messageBot;

    private BattleManager _battleManager;
    private Controller _controller1;
    private Controller _controller2;
    
    protected override void Awake()
    {
        base.Awake();
        Dispatcher.OnTutorButtonEndTurnLock += OnTurnButtonTutorReaction;
        
        _btnTurn.onClick.AddListener(OnClickTurn);
    }

    private void OnDestroy()
    {
        Dispatcher.OnTutorButtonEndTurnLock -= OnTurnButtonTutorReaction;
        _battleManager.Unsubscribe();
        
        if (_controller1 != null) _controller1.OnEndBattle -= HideTurnButton;
    }

    public override void Init(params object[] param)
    {
        Game.Instance.SetBattleCanvas(_canvas);
        
        _controller1 = (Controller)param[0];
        _controller2 = (Controller)param[1];

        Dispatcher.OnBattleRestart += Ready;
        
        Ready();
    }

    public override void Show()
    {
        base.Show();
        //Audio.Play(Sound.UI_PANEL_SHOW);
    }

    public override void Hide()
    {
        base.Hide();
        //Audio.Play(Sound.UI_PANEL_HIDE);
        _battleManager.OnTurn -= UIUpdate;
        Dispatcher.OnBattleRestart -= Ready;
    }

    private void Ready()
    {
        _controller1.SetCommander(_player);
        _controller2.SetCommander(_enemy);

        _controller1.OnEndBattle += HideTurnButton;
        _btnTurn.onClick.AddListener(() =>
        {
            //_lblBtnTurn.GetComponent<LocalizedTextMP>().Key = "UI_BUTTON_END_TURN";
            UIUpdateAI(Game.Instance.LocalizationManager.GetTranslate("UI_BUTTON_END_TURN"));
            _controller1.Turn();
        });

        _battleManager = new BattleManager(_controller1, _controller2);
        _battleManager.OnTurn += UIUpdate;
        _battleManager.Run();
        
        //_messageBot.SetActive(false);
    }

    private void HideTurnButton()
    {
        _btnTurn.gameObject.SetActive(false);
        //_messageBot.SetActive(false);
    }

    private void UIUpdate(ControllerType controllerType)
    {
        switch (controllerType)
        {
            case ControllerType.AI:
                /*_messageBot.SetActive(true);
                _lblBtnTurn.GetComponent<LocalizedTextMP>().Key = "UI_BUTTON_END_TURN_ENEMY";*/
                UIUpdateAI(Game.Instance.LocalizationManager.GetTranslate("UI_BUTTON_END_TURN_ENEMY"));
                break;
            case ControllerType.PLAYER:
                //_messageBot.SetActive(false);
                _btnTurn.gameObject.SetActive(true);
                //_lblBtnTurn.GetComponent<LocalizedTextMP>().Key = "UI_BUTTON_END_TURN";
                //_lblBtnTurn.text = Game.Instance.LocalizationManager.GetTranslate("UI_BUTTON_END_TURN");
                _btnTurn.interactable = true;
                //_btnTurnIcon.color = _btnTurnIconC1;
                _maskPlayer.SetActive(false);
                _maskField.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
        }
    }

    private void UIUpdateAI(string value)
    {
        _btnTurn.interactable = false;
        //_btnTurnIcon.color = _btnTurnIconC2;
        //_lblBtnTurn.text = value;
        _maskPlayer.SetActive(true);
        _maskField.SetActive(true);
    }

    private void OnTurnButtonTutorReaction(object arg)
    {
        var interactable = (bool)arg;
        _btnTurn.interactable = interactable;
        //_btnTurnIcon.color = interactable ? _btnTurnIconC1 : _btnTurnIconC2;
    }

    private void OnClickTurn()
    {
        //Audio.Play(Sound.UI_BUTTON_CLICK_MUENU);
    }
}
