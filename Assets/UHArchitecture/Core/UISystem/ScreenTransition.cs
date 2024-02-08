using System;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private TransitionEffect _transitionEffect;

    public void Perform(Action callback, TransitionMode transitionMode = TransitionMode.SHOW_HIDE)
    {
        var effect = Instantiate(_transitionEffect, transform);
        effect.Init(callback, transitionMode);
    }

    public void Hide()
    {
        transform.GetChild(0).GetComponent<TransitionEffect>().OnHide();
    }
    
    public void ShowLabelNext()
    {
        transform.GetChild(0).GetComponent<TransitionEffect>().ShowLabelNext();
    }
}

public enum TransitionMode
{
    SHOW_HIDE,
    HIDE,
}
