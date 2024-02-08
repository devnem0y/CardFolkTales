using System;
using System.Collections;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _labelNext;
    
    private Action _callback;
    
    public void Init(Action callback, TransitionMode transitionMode)
    {
        if (transitionMode == TransitionMode.SHOW_HIDE)
        {
            _callback = callback;
            _animator.Play("Show");   
        }
        else
        {
            _animator.Play("Normal");
            callback?.Invoke();
        }
    }

    private void AnimationEvent()
    {
        _callback?.Invoke();
        StartCoroutine(HideDelay(true));
    }
    
    private IEnumerator HideDelay(bool isDelay)
    {
        yield return new WaitForSeconds(isDelay ? _delay : 0.25f);
        _animator.Play("Hide");
        var currentAnimationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, currentAnimationTime + 0.15f);
    }

    public void OnHide()
    {
        _labelNext.SetActive(false);
        StartCoroutine(HideDelay(false));
    }

    public void ShowLabelNext()
    {
        _labelNext.SetActive(true);
    }
}
