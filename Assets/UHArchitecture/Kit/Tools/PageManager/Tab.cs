using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UralHedgehog
{
    [RequireComponent(typeof(Image), typeof(AudioComponent))]
    public class Tab : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public event Action<int> onEnable;
        [HideInInspector] public event Action onClick;

        [SerializeField] private Sprite _available;
        [SerializeField] private Sprite _selected;

        [SerializeField] private Image _icon; //TODO: расширение с иконкой
        [SerializeField] private TMP_Text _label; //TODO: расширение с текстом
        [SerializeField] private List<Color> _labelColors; //TODO: расширение с текстом
        [SerializeField] private List<Color> _buttonColors; //TODO: расширение с цветом кнопки

        [SerializeField] private bool _interactable = true;
        public bool Interactable => _interactable;

        //private Animator _animator;
        private Image _target;
        private AudioComponent _audio;

        private State _state;
        private int _pageId;

        private bool _isInit;

        private void Awake()
        {
            //_animator = GetComponent<Animator>();
            _target = GetComponent<Image>();
            _audio = GetComponent<AudioComponent>();
            _audio.IsUIElement = true;

            Dispatcher.ChangeStateTabs += ChangeStateFix;
        }

        private void OnDestroy()
        {
            Dispatcher.ChangeStateTabs -= ChangeStateFix;
        }

        public void Init(int pageId)
        {
            _pageId = pageId;
        }

        public void ChangeState(State state)
        {
            if (_state == state && _isInit) return;

            _state = state;

            switch (_state)
            {
                case State.AVAILABLE:
                    if (!_isInit) _isInit = true;
                    if (_target != null && _available != null)
                    {
                        _target.sprite = _available;
                        _target.color = _buttonColors[0];
                    }
                    SetInteractable(_interactable);
                    break;
                case State.SELECTED:
                    if (!_isInit) _isInit = true;
                    if (_target != null && _selected != null)
                    {
                        _target.sprite = _selected;
                        _target.color = _buttonColors[1];
                        if (_label != null) _label.color = _labelColors[_interactable ? 2 : 1];
                    }
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_state == State.SELECTED || !_interactable) return;

            _audio.Play(Sound.UI_BUTTON_CLICK_0);
            onEnable?.Invoke(_pageId);
            onClick?.Invoke();
        }
        
        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
            
            if (_target != null) _target.raycastTarget = interactable;
            if (_label != null) _label.color = _labelColors[interactable ? 0 : 1];
        }

        private void ChangeStateFix()
        {
            ChangeState(State.AVAILABLE);
        }

        /*private IEnumerator PlayAnimationToAnimation(string animation1, string animation2)
        {
            _animator.Play(animation1);
            var currentAnimationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(currentAnimationTime);
            _animator.Play(animation2);
        }*/

        public enum State
        {
            AVAILABLE,
            SELECTED,
        }
    }
}