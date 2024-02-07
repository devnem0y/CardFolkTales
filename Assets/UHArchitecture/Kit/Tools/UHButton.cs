using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UralHedgehog
{
    public class UHButton : Button
    {
        public bool Pointed { get; private set; }
        public event Action<bool> onPointed;  
    
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Pointed = true;
            onPointed?.Invoke(Pointed);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Pointed = false;
            onPointed?.Invoke(Pointed);
        }
    }
}
