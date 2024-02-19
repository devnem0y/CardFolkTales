using System;
using UnityEngine;
using UralHedgehog.UI;

namespace UralHedgehog
{
    public class Game : Bootstrap
    {
        public static Game Instance { get; private set; }

        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private Sprite _bgMain;
        [SerializeField] private Sprite _bgBattle;

        private bool _isFirstLaunch;
        private bool _isBegin;

        public IPlayer Player => _player;

        private void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            _isFirstLaunch = true;
            Run();
        }

        protected override void Initialization()
        {
            base.Initialization();
        }

        protected override void OnBegin()
        {
            _background.sprite = _bgMain;
            PanelMenu(true);
            ScreenMain(true);
            ScreenTransition.ShowLabelNext();
            _isBegin = true;
        }

        public override void ChangeState(GameState state)
        {
            base.ChangeState(state);
            
            switch (GameState)
            {
                case GameState.LOADING:
                    Debug.Log("<color=yellow>Loading</color>");
                    ScreenTransition.Perform(null, TransitionMode.HIDE); 
                    break;
                case GameState.MAIN:
                    Debug.Log("<color=yellow>Main</color>");
                    if (!_isFirstLaunch)
                    {
                        ScreenTransition.Perform(() =>
                        {
                            _background.sprite = _bgMain;
                            ScreenBattle(false);
                            ScreenMain(true);
                        });   
                    }
                    else
                    {
                        _isFirstLaunch = false;
                    }
                    break;
                case GameState.PLAY:
                    Debug.Log("<color=yellow>Play</color>");
                    ScreenTransition.Perform(() =>
                    {
                        _background.sprite = _bgBattle;
                        ScreenMain(false);
                        ScreenBattle(true);
                    });
                    break;
                case GameState.VICTORY:
                    Debug.Log("<color=yellow>Victory</color>");
                    break;
                case GameState.DEFEAT:
                    Debug.Log("<color=yellow>Defeat</color>");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            if (_isBegin)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    _isBegin = false;
                    ScreenTransition.Hide();
                    ChangeState(GameState.MAIN);
                }
            }
        }

        private static void ScreenMain(bool show)
        {
            var pMainPlayerInfoData = new Data(nameof(PMainPlayerInfo));
            var pMainBottom = new Data(nameof(PMainBottom));
            
            UIDispatcher.Send(show ? EventUI.SHOW_WIDGET : EventUI.HIDE_WIDGET, pMainPlayerInfoData);
            UIDispatcher.Send(show ? EventUI.SHOW_WIDGET : EventUI.HIDE_WIDGET, pMainBottom);
        }

        private static void ScreenBattle(bool show)
        {
            var pBattle = new Data(nameof(PBattle));
            
            UIDispatcher.Send(show ? EventUI.SHOW_WIDGET : EventUI.HIDE_WIDGET, pBattle);
        }

        private static void PanelMenu(bool show)
        {
            var pMenuData = new Data(nameof(PMenu));
            UIDispatcher.Send(show ? EventUI.SHOW_WIDGET : EventUI.HIDE_WIDGET, pMenuData);
        }
    }
}