using System;
using UnityEngine;

namespace UralHedgehog
{
    namespace UI
    {
        public abstract class Widget : MonoBehaviour, IWidget
        {
            [SerializeField] private Type _type;
            public Type Type => _type;

            private Transform _wrapper;
            
            protected virtual void Awake()
            {
                _wrapper = transform.GetChild(0);
                _wrapper.localScale = Vector3.zero;
            }

            public virtual void Init()
            {
                
            }

            public virtual void Init(params object[] param)
            {
                
            }

            public virtual void Show()
            {
                _wrapper.localScale = Vector3.one;
            }

            public virtual void Hide()
            {
                UIDispatcher.Send(EventUI.KILL, this);
                Destroy(gameObject);
            }
            
            protected static void Open(Action callback)
            {
                callback?.Invoke();
            }
            
            protected static void Close(Action callback)
            {
                callback?.Invoke();
            }
        }

        public enum Type
        {
            PANEL = 0,
            WINDOW = 1,
        }
        
        public class Data
        {
            public string Name { get; }
            public object[] Params { get; }

            public Data(string name)
            {
                Name = name;
            }
        
            public Data(string name, params object[] param)
            {
                Name = name;
                Params = param;
            }
        }
    }
}