using System;

namespace UralHedgehog
{
    namespace UI
    {
        public static class UIDispatcher
        {
            #region Events

            public static event Action<object> ShowWidget;
            public static event Action<object> HideWidget;
            public static event Action<object> Kill;

            #endregion

            #region ActionsEventHasParam

            private static Action<object> GetEventHasParam(EventUI e)
            {
                return e switch
                {
                    EventUI.SHOW_WIDGET => ShowWidget,
                    EventUI.HIDE_WIDGET => HideWidget,
                    EventUI.KILL => Kill,
                    
                    _ => throw new ArgumentOutOfRangeException(nameof(e), e, null)
                };
            }

            #endregion

            #region Send
            
            /// <summary>
            /// Отправка события с одним параметром любого типа
            /// </summary>
            /// <param name="e">Событие</param>
            /// <param name="arg">Параметр</param>
            public static void Send(EventUI e, object arg)
            {
                Invoker(GetEventHasParam(e), arg);
            }

            /// <summary>
            /// Отправка события с массивом параметров любого типа
            /// </summary>
            /// <param name="e">Событие</param>
            /// <param name="args">Массив параметров</param>
            public static void Send(EventUI e, params object[] args)
            {
                Invoker(GetEventHasParam(e), args);
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

        public enum EventUI
        {
            SHOW_WIDGET,
            HIDE_WIDGET,
            KILL,
        }
    }
}