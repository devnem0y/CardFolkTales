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
        public static event Action<object> OnShowDialog;

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
                EventD.ON_SHOW_DIALOG => OnShowDialog,

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
        ON_SHOW_DIALOG,

        // UI События для пользовательского интерфейса
        UI_CHANGE_STATE_TABS,
    }
}