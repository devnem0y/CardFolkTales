using System;
using System.Collections;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private Action _callback;
    private float _delay;
    
    public void Init(Action callback, float delay)
    {
        _callback = callback;
        _delay = delay;
        _animator.Play("Show");
    }

    private void AnimationEvent()
    {
        _callback?.Invoke();
        StartCoroutine(HideDelay());
    }
    
    private IEnumerator HideDelay()
    {
        yield return new WaitForSeconds(_delay);
        _animator.Play("Hide");
        var currentAnimationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, currentAnimationTime - 0.13f);
    }
}
