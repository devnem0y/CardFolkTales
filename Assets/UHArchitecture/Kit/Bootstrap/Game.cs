using System;
using System.Globalization;
using UnityEngine;
using UralHedgehog.UI;

namespace UralHedgehog
{
    public class Game : Bootstrap
    {
        public static Game Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            Run();
        }

        protected override void Initialization()
        {
            base.Initialization();
        }

        public override void ChangeState(GameState state)
        {
            base.ChangeState(state);
            
            switch (GameState)
            {
                case GameState.LOADING:
                    Debug.Log("<color=yellow>Loading</color>");
                    break;
                case GameState.MAIN:
                    Debug.Log("<color=yellow>Main</color>");
                    OpenViewSettings();
                    break;
                case GameState.PLAY:
                    Debug.Log("<color=yellow>Play</color>");
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
    }
}