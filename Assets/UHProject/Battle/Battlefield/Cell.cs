using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UralHedgehog;

public class Cell : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool _isPlayer;
    [SerializeField] private CellType _type;
    [SerializeField] private Image _image;
    [SerializeField] private Color _default;
    [SerializeField] private Color _lock;
    [SerializeField] private Color _available;
    
    public CellType Type => _type;
    public Card Card { get; private set; }
    public bool IsValid { get; private set; } // Доступность по типу карта / ячейка
    public bool IsEmpty { get; private set; }

    public void OnDrop(PointerEventData eventData)
    {
        if (!_isPlayer) return;
        if (!IsEmpty) return;
        if (!IsValid) return;

        var otherCard = eventData.pointerDrag.transform;
        var card = otherCard.GetComponent<Card>();

        SetupCard(card, ControllerType.PLAYER);

        if (Game.Instance.TutorialHandler.IsActualTutorialStep(0, 1)) // Второй шаг тутора ставим бмп
        {
            Game.Instance.TutorialHandler.Complete(0, 1);
            new Dialog(Game.Instance.TutorialsDialogsData[2]);
        }
        
        if (Game.Instance.TutorialHandler.IsActualTutorialStep(0, 0)) // Первый шаг тутора ставим пеходу
        {
            Game.Instance.TutorialHandler.Complete(0, 0);
            new Dialog(Game.Instance.TutorialsDialogsData[3], 
                () => Dispatcher.Send(EventD.ON_TUTOR_BUTTON_END_TURN_LOCK, true));
        }
    }
    
    public void SetupCard(Card card, ControllerType controllerType)
    {
        Card = card;
        
        if (Card == null) return;
        
        if (Card.IsCell)
        {
            Card = null;
            IsEmpty = true;
            return;
        }
            
        Card.Get<Unit>().OnDestroyed += Clear;
        
        Card.transform.SetParent(transform);
        var rectTransform = Card.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(90f, 120f);
        if (!_isPlayer) rectTransform.localScale = new Vector3(1f, 1f, 1f);
        Card.transform.localPosition = Vector3.zero;
        
        IsEmpty = false;
        
        Card.Use(controllerType);
    }

    public void CheckValid(UnitType unitType)
    {
        IsValid = unitType switch
        {
            UnitType.WARRIOR => _type == CellType.WA,
            UnitType.ARCHER => _type is CellType.WA or CellType.AM,
            UnitType.MAGICIAN => _type is CellType.AM or CellType.M,
            _ => throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null)
        };

        if (IsValid)
        {
            //TODO: Валидное выделение
            _image.color = _available;
            
            if (!IsEmpty) _image.color = _lock;
        }
        else
        {
            //TODO: Залоченое выделение
            _image.color = _lock;
        }
    }

    protected internal void RemoveSelection()
    {
        //TODO: При отпускании карты снимать выделение
        _image.color = _default;
    }

    public void Clear()
    {
        if (Card != null)
        {
            Card.Get<Unit>().OnDestroyed -= Clear;
            Card = null;
        }
        
        IsEmpty = true;
    }
}

public enum CellType
{
    WA = 0, // Воин / Лучник
    AM = 1, // Лучник / Волшебник
    M = 2, // Волшебник / Колдун / Маг / Чародей
}
