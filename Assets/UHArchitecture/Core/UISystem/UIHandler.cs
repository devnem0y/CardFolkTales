using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UralHedgehog
{
    namespace UI
    {
        public class UIHandler : MonoBehaviour
        {
            [SerializeField] private Transform _wrapperPanels;
            [SerializeField] private Transform _wrapperWindows;
            [SerializeField] private Widget[] _widgets;

            private List<Widget> _list;

            private void Awake()
            {
                _list = new List<Widget>();

                UIDispatcher.ShowWidget += Create;
                UIDispatcher.HideWidget += Hide;
                UIDispatcher.Kill += Kill;
            }

            private void OnDestroy()
            {
                UIDispatcher.ShowWidget -= Create;
                UIDispatcher.HideWidget -= Hide;
                UIDispatcher.Kill -= Kill;
            }

            private void Create(object arg)
            {
                var data = (Data)arg;

                foreach (var w in _widgets)
                {
                    if (!w.name.Equals(data.Name)) continue;
                    var widget = Instantiate(w, w.Type == Type.WINDOW ? _wrapperWindows : _wrapperPanels);
                    widget.gameObject.name = data.Name;
                    var WidgetComponent = (Widget)widget.GetComponent(data.Name);
                    _list.Add(WidgetComponent);
                    if (data.Params != null) widget.Init(data.Params);
                    else widget.Init();
                    widget.Show();
                }
            }

            private void Hide(object arg)
            {
                var data = (Data)arg;
                var widget = GetWidget(_list, data.Name);
                if (widget == null) return;
                widget.Hide();
            }

            private void Kill(object arg)
            {
                var widget = (Widget)arg;
                _list.Remove(widget);
            }

            private static Widget GetWidget(IEnumerable<Widget> list, string name)
            {
                return list.FirstOrDefault(window => window.GetType().Name.Equals(name));
            }
        }
    }
}