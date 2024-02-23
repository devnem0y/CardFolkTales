using System;

namespace UralHedgehog
{
    /// <summary>
    /// Добавить событие в enum, затем в регион Events
    /// После сделать зависимость в нужном из регионов (ActionsEvent / ActionsEventHasParam)
    /// Если событие имеет параметр, то всегда передается object (Action<object>)
    /// </summary>
    public class Dispatcher
    {
        #region Events

        // SYSTEM Не игровые события, а глобальные для проекта
        public static event Action SystemLoading;
        public static event Action SystemLaunch;
        public static event Action SystemBegin;
        public static event Action SystemLocalize;

        // ON Игровые события
        public static event Action OnCountersUpdate;
        public static event Action<object> OnChangeGameState;
        public static event Action<object> OnCardSelected;
        public static event Action<object> OnCardUsed;
        public static event Action OnCardDeselect;
        public static event Action OnLeaveFight;
        public static event Action<object> OnCardAdd;
        public static event Action<object> OnRewardCardAdd;
        public static event Action<object> OnCardRemove;
        public static event Action<object> OnSetColorPlayerTeam;
        public static event Action OnDeckUpdate;
        public static event Action OnCollectionUpdate;
        public static event Action OnBattleRestart;
        public static event Action OnLevelComplete;
        public static event Action<object> OnShowDialog;
        public static event Action OnTutorKMBComplete;
        public static event Action<object> OnTutorButtonEndTurnLock;
        public static event Action OnReactionCallbackFullscreenAdv;
        public static event Action OnAdvReward;
        public static event Action<object> OnInAppPurchase;

        // UI События для пользовательского интерфейса
        public static event Action ChangeStateTabs;

        #endregion

        #region ActionsEvent

        private static Action GetEvent(EventD e)
        {
            return e switch
            {
                EventD.SYSTEM_LOADING => SystemLoading,
                EventD.SYSTEM_LAUNCH => SystemLaunch,
                EventD.SYSTEM_BEGIN => SystemBegin,
                EventD.SYSTEM_LOCALIZE => SystemLocalize,
                
                EventD.ON_COUNTERS_UPDATE => OnCountersUpdate,
                EventD.ON_CARD_DESELECT => OnCardDeselect,
                EventD.ON_LEAVE_FIGHT => OnLeaveFight,
                EventD.ON_DECK_UPDATE => OnDeckUpdate,
                EventD.ON_COLLECTION_UPDATE => OnCollectionUpdate,
                EventD.ON_BATTLE_RESTART => OnBattleRestart,
                EventD.ON_LEVEL_COMPLETE => OnLevelComplete,
                EventD.ON_TUTOR_KMB_COMPLETE => OnTutorKMBComplete,
                EventD.ON_REACTION_CALLBACK_FULLSCREEEN_ADV => OnReactionCallbackFullscreenAdv,
                EventD.ON_ADV_REWARD => OnAdvReward,
                
                EventD.UI_CHANGE_STATE_TABS => ChangeStateTabs,
                
                _ => throw new ArgumentOutOfRangeException(nameof(e), e, null)
            };
        }

        #endregion

        #region ActionsEventHasParam

        private static Action<object> GetEventHasParam(EventD e)
        {
            return e switch
            {
                EventD.ON_CHANGE_GAME_STATE => OnChangeGameState,
                EventD.ON_CARD_SELECTED => OnCardSelected,
                EventD.ON_CARD_USED => OnCardUsed,
                EventD.ON_CARD_ADD => OnCardAdd,
                EventD.ON_REWARD_CARD_ADD => OnRewardCardAdd,
                EventD.ON_CARD_REMOVE => OnCardRemove,
                EventD.ON_SET_COLOR_PLAYER_TEAM => OnSetColorPlayerTeam,
                EventD.ON_SHOW_DIALOG => OnShowDialog,
                EventD.ON_TUTOR_BUTTON_END_TURN_LOCK => OnTutorButtonEndTurnLock,
                EventD.ON_IN_APP_PURCHASE => OnInAppPurchase,

                _ => throw new ArgumentOutOfRangeException(nameof(e), e, null)
            };
        }

        #endregion

        #region Send

        /// <summary>
        /// Отправка события без параметров
        /// </summary>
        /// <param name="e">Событие</param>
        public static void Send(EventD e)
        {
            Invoker(GetEvent(e));
        }

        /// <summary>
        /// Отправка события с одним параметром любого типа
        /// </summary>
        /// <param name="e">Событие</param>
        /// <param name="arg">Параметр</param>
        public static void Send(EventD e, object arg)
        {
            Invoker(GetEventHasParam(e), arg);
        }

        /// <summary>
        /// Отправка события с массивом параметров любого типа
        /// </summary>
        /// <param name="e">Событие</param>
        /// <param name="args">Массив параметров</param>
        public static void Send(EventD e, params object[] args)
        {
            Invoker(GetEventHasParam(e), args);
        }

        private static void Invoker(Action action)
        {
            action?.Invoke();
        }

        private static void Invoker(Action<object> action, object arg)
        {
            action?.Invoke(arg);
        }

        private static void Invoker(Action<object> action, params object[] args)
        {
            action?.Invoke(args);
        }

        #endregion
    }

    public enum EventD
    {
        // SYSTEM Не игровые события, а глобальные для проекта
        SYSTEM_LOADING,
        SYSTEM_LAUNCH,
        SYSTEM_BEGIN,
        SYSTEM_LOCALIZE,

        // ON Игровые события
        ON_COUNTERS_UPDATE,
        ON_CHANGE_GAME_STATE,
        ON_CARD_SELECTED,
        ON_CARD_DESELECT,
        ON_CARD_USED,
        ON_CARD_ADD,
        ON_REWARD_CARD_ADD,
        ON_LEAVE_FIGHT,
        ON_SET_COLOR_PLAYER_TEAM,
        ON_DECK_UPDATE,
        ON_CARD_REMOVE,
        ON_COLLECTION_UPDATE,
        ON_BATTLE_RESTART,
        ON_LEVEL_COMPLETE,
        ON_SHOW_DIALOG,
        ON_TUTOR_KMB_COMPLETE,
        ON_TUTOR_BUTTON_END_TURN_LOCK,
        ON_REACTION_CALLBACK_FULLSCREEEN_ADV,
        ON_ADV_REWARD,
        ON_IN_APP_PURCHASE,

        // UI События для пользовательского интерфейса
        UI_CHANGE_STATE_TABS,
    }
}