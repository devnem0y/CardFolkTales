using System;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UralHedgehog.UI;
using Random = UnityEngine.Random;

namespace UralHedgehog
{
    public class Game : Bootstrap
    {
        public static Game Instance { get; private set; }

        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private Sprite _bgMain;
        [SerializeField] private Sprite _bgBattle;
        [SerializeField] private Canvas _battleCanvas;
        
        [Space(5)] [Header("Хранилища (SO)")]
        [SerializeField] private CardStorage _cardStorage;
        [SerializeField] private BotsStorage _levelBotsStorage;
        [SerializeField] private List<BotsStorage> _randomBotStorages;
        //[SerializeField] private ProductStorage _productStorage;
        [SerializeField] private TutorialStorage _tutorialStorage;
        
        [Space(5)] [Header("Диалоги для тутора (SO)")] 
        [SerializeField] private List<DialogData> _tutorialsDialogsData;

        private TutorialHandler _tutorialHandler;

        private bool _isFirstLaunch;
        private bool _isBegin;
        private bool _isBattle;

        public ITutorialHandler TutorialHandler => _tutorialHandler;
        public List<DialogData> TutorialsDialogsData => _tutorialsDialogsData;
        public IPlayer Player => _player;
        
        public float CanvasScale => _battleCanvas.scaleFactor;

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
            _tutorialHandler = new TutorialHandler(_player.TutorialsData, _tutorialStorage, _tutorialsDialogsData);
            _player.SetCardStorage(_cardStorage);
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
                        if (_isBattle) ScreenBattle(false);
                        ScreenBattle(true);
                    });
                    break;
                case GameState.VICTORY:
                    Debug.Log("<color=yellow>Victory</color>");
                    ShowPanelLoseWin();
                    break;
                case GameState.DEFEAT:
                    Debug.Log("<color=yellow>Defeat</color>");
                    ShowPanelLoseWin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetBattleCanvas(Canvas canvas)
        {
            _battleCanvas = canvas;
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
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (GameState == GameState.PLAY)
                {
                    ChangeState(GameState.VICTORY);
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

        private void ScreenBattle(bool show)
        {
            var bot = new Bot(_randomBotStorages[0].Bots[Random.Range(0, _randomBotStorages.Count - 1)], _cardStorage);
            var pBattle = new Data(nameof(PBattle), _player, bot);

            _isBattle = show;
            
            UIDispatcher.Send(show ? EventUI.SHOW_WIDGET : EventUI.HIDE_WIDGET, pBattle);
        }

        private static void PanelMenu(bool show)
        {
            var pMenuData = new Data(nameof(PMenu));
            UIDispatcher.Send(show ? EventUI.SHOW_WIDGET : EventUI.HIDE_WIDGET, pMenuData);
        }

        private void ShowPanelLoseWin()
        {
            var pLoseWin = new Data(nameof(PLoseWin));
            UIDispatcher.Send(EventUI.SHOW_WIDGET, pLoseWin);
        }
    }
}