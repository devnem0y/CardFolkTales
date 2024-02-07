using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UralHedgehog
{
    [RequireComponent(typeof(Image), typeof(AudioComponent))]
    public class UHSimpleButton : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public event Action onClick;
        
        [SerializeField] private Sprite _available;
        [SerializeField] private Sprite _deselect;

        [SerializeField] private Image _icon; //TODO: расширение с иконкой
        [SerializeField] private TMP_Text _label; //TODO: расширение с текстом
        [SerializeField] private List<Color> _labelColors; //TODO: расширение с текстом

        [SerializeField] private bool _interactable = true;
        public bool Interactable => _interactable;
        
        private Image _target;
        private AudioComponent _audio;

        private void Awake()
        {
            _target = GetComponent<Image>();
            _audio = GetComponent<AudioComponent>();

            SetInteractable(_interactable);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_interactable) return;
            
            _audio.Play(Sound.UI_BUTTON_CLICK_0);
            onClick?.Invoke();
        }

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
            
            if (_target != null)
            {
                _target.sprite = _interactable ? _available : _deselect;
                _target.raycastTarget = _interactable;
            }
            
            if (_label != null) _label.color = _labelColors[_interactable ? 0 : 1];
        }
    }
}