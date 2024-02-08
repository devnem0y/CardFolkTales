using System;
using System.Globalization;
using UnityEngine;
using UralHedgehog.UI;

namespace UralHedgehog
{
    public class Game : Bootstrap
    {
        public static Game Instance { get; private set; }

        private bool _isFirstLaunch;
        private bool _isBegin;

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
            OpenViewSettings();
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
                        ScreenTransition.Perform(OpenViewSettings);   
                    }
                    else
                    {
                        _isFirstLaunch = false;
                    }
                    break;
                case GameState.PLAY:
                    Debug.Log("<color=yellow>Play</color>");
                    ScreenTransition.Perform(OpenViewSettings);
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
    }
}