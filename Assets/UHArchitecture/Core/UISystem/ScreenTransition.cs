using System;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private TransitionEffect _transitionEffect;

    public void Perform(Action callback, float delay = 1f)
    {
        var effect = Instantiate(_transitionEffect, transform);
        effect.Init(callback, delay);
    }
}
