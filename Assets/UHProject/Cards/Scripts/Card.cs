using System;
using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UralHedgehog;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private const string ANIM_BONUS_DESTROY_NAME = "Setuping";
    
    [SerializeField] private CardType _type;

    [SerializeField] private Animator _animator;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cover;
    [SerializeField] private Image _grayMask;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _lblTurnPoints;
    
    [SerializeField, ShowIf(nameof(_type), CardType.BONUS), AllowNesting]
    private TMP_Text _lblDescription;
    [SerializeField, ShowIf(nameof(_type), CardType.UNIT), AllowNesting]
    private ParamGroup _paramGroupLeft;
    [SerializeField, ShowIf(nameof(_type), CardType.UNIT), AllowNesting]
    private ParamGroup _paramGroupRight;
    
    [SerializeField] private FlyText vfx;
    [SerializeField] private AudioComponent _audio;

    private CommanderBase _commander;
    private CardBase _cardBase;
    private Unit _unit;
    private Bonus _bonus;
    
    private Transform _parent;
    private RectTransform _rectTransformTmp;
    private int _index;
    
    public bool IsCell { get; private set; }
    public int TurnPoints { get; private set; }

    public void Init(CardBase cardBase, Color colorTeam, RectTransform rectTransformTmp, CommanderBase commander)
    {
        _commander = commander;
        _cardBase = cardBase;
        TurnPoints = _cardBase.TurnPoints;
        IsCell = false;
        
        _name.text = Game.Instance.LocalizationManager.GetTranslate(_cardBase.Name);
        _name.GetComponent<LocalizedTextMP>().Key = _cardBase.Name;
        _lblTurnPoints.text = $"{TurnPoints}";
        _icon.sprite = _cardBase.Icon;
        _cover.color = colorTeam;
        if (_cardBase.Type == CardType.UNIT) _icon.color = colorTeam;

        if (_lblDescription != null)
        {
            var d1 = Game.Instance.LocalizationManager.GetTranslate(_cardBase.Description);
            _lblDescription.text = _cardBase.Magnitude > 0 ? $"{d1}\n+{_cardBase.Magnitude}" : $"<size=9>{d1}";
            var localizeText = _lblDescription.GetComponent<LocalizedTextMP>();
            if (_cardBase.Magnitude > 0) localizeText.Param = $"+{_cardBase.Magnitude}";
            else localizeText.Prefix = "<size=9>";
            localizeText.Key = _cardBase.Description;
        }

        _rectTransformTmp = rectTransformTmp;

        switch (_type)
        {
            case CardType.UNIT:
                CreateUnit(_cardBase);
                break;
            case CardType.BONUS:
                _bonus = new Bonus(_cardBase);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public T Get<T>()
    {
        if (typeof(T) == typeof(Unit)) return (T)(object)_unit;
        if (typeof(T) == typeof(Bonus)) return (T)(object)_bonus;
        
        return default;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_type == CardType.UNIT)
        {
            if (IsCell) return;
            
            //Dispatcher.Send(Event.ON_CARD_SELECTED, _cardBase.UnitType);
            DragSetup();
        }
        else
        {
            //Dispatcher.Send(Event.ON_CARD_SELECTED, _cardBase.BonusType);
            DragSetup();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_type == CardType.UNIT)
        {
            //if (!IsCell) _rectTransform.anchoredPosition += eventData.delta / Game.Instance.CanvasScale;
        }
        else
        {
            //_rectTransform.anchoredPosition += eventData.delta / Game.Instance.CanvasScale;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var other = eventData.pointerCurrentRaycast.gameObject.transform;
        
        if (_type == CardType.UNIT)
        {
            //Dispatcher.Send(Event.ON_CARD_DESELECT);

            if (!IsCell)
            {
                if (other.CompareTag("Cell"))
                {
                    var cell = other.GetComponent<Cell>();
                
                    if (!cell.IsValid) RollBack();
                }
                else
                {
                    RollBack();
                }
            }
        }
        else
        {
            if (other.GetComponent<Card>() == null)
            {
                RollBack();
            }
            else
            {
                if (!other.GetComponent<Card>().IsCell)
                {
                    RollBack();
                }
                else
                {
                    if (!other.GetComponent<Card>().Get<Unit>().IsValid)
                    {
                        RollBack();
                    }
                }
            }
        }

        _canvasGroup.blocksRaycasts = true;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (_type != CardType.UNIT) return;
        
        if (!IsCell) return;
        if (!_unit.IsValid) return;
        
        var otherCard = eventData.pointerDrag.transform;
        var card = otherCard.GetComponent<Card>();
        var bonus = card.Get<Bonus>();

        if (bonus == null) return;
        
        _unit.SetupBonus(card, _audio);
    }
    
    public void Use(ControllerType controllerType)
    {
        switch (controllerType)
        {
            case ControllerType.AI:
                switch (_type)
                {
                    case CardType.UNIT:
                        IsCell = true;
                        //_audio.Play(Sound.USE_UNIT_CARD);
                        break;
                    case CardType.BONUS:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ControllerType.PLAYER:
                switch (_type)
                {
                    case CardType.UNIT:
                        IsCell = true;
                        //_audio.Play(Sound.USE_UNIT_CARD);
                        break;
                    case CardType.BONUS:
                        Destroyed();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
        }
        
        _commander.InvokeCardUse(_cardBase);
    }

    public void Use(ControllerType controllerType, Transform cell, Action callback)
    {
        switch (controllerType)
        {
            case ControllerType.AI:
                switch (_type)
                {
                    case CardType.UNIT:
                        //_audio.Play(Sound.USE_UNIT_CARD);
                        break;
                    case CardType.BONUS:
                        AnimBonusCard(cell, callback);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ControllerType.PLAYER:
                switch (_type)
                {
                    case CardType.UNIT:
                        //_audio.Play(Sound.USE_UNIT_CARD);
                        break;
                    case CardType.BONUS:
                        Destroyed();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
        }
        
        _commander.InvokeCardUse(_cardBase);
    }

    public void CoverUp(bool active)
    {
        _cover.gameObject.SetActive(active);
        _lblTurnPoints.transform.parent.gameObject.SetActive(!active);
    }

    public void StateUpdate(int turnPoints)
    {
        if (_cardBase.TurnPoints <= turnPoints)
        {
            _canvasGroup.blocksRaycasts = true;
            _grayMask.gameObject.SetActive(false);
        }
        else
        {
            _canvasGroup.blocksRaycasts = false;
            _grayMask.gameObject.SetActive(true);
        }
    }

    private void DragSetup()
    {
        _parent = _rectTransform.parent;
        _index = _rectTransform.GetSiblingIndex();
        _rectTransform.SetParent(_rectTransformTmp);
        _canvasGroup.blocksRaycasts = false;
    }

    private void CreateUnit(CardBase cardBase)
    {
        _unit = new Unit(cardBase);
        _paramGroupLeft.GetMarker(MarkerType.HP).SetValue(cardBase.Hp);
        _paramGroupRight.GetMarker(MarkerType.ATTACK).SetValue(cardBase.Attack);

        if (cardBase.Defense > 0)
        {
            _paramGroupLeft.GetMarker(MarkerType.DEFENSE)?.SetValue(cardBase.Defense);
            _paramGroupLeft.SetVisible(MarkerType.DEFENSE, true);
        }

        _unit.CheckBonuses();

        _unit.OnDestroyed += Destroyed;
        _unit.OnSetBonus += MarkerBonusUpdate;
        _unit.OnDealsDamage += AnimationDealsDamage;
        _unit.OnFail += AnimFail;
        _unit.OnTakingDamage += MarkerHpUpdate;
        _unit.OnRemoveDefense += AnimRemoveDefense;
    }

    private void Destroyed()
    {
        //TODO: Проигрываем анимацию, отписываемся от событий, уничтожаем объект
        switch (_type)
        {
            case CardType.UNIT:
                StartCoroutine(Delay(SetupExplosionEffect,
                    () =>
                    {
                    _unit.OnDestroyed -= Destroyed;
                    _unit.OnSetBonus -= MarkerBonusUpdate;
                    _unit.OnDealsDamage -= AnimationDealsDamage;
                    _unit.OnFail -= AnimFail;
                    _unit.OnTakingDamage -= MarkerHpUpdate;
                    _unit.OnRemoveDefense -= AnimRemoveDefense;
                    Destroy(gameObject);
                    }, 1.2f));
                break;
            case CardType.BONUS:
                Destroy(gameObject);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void MarkerBonusUpdate(BonusType bonusType, int value, int delta)
    {
        switch (bonusType)
        {
            case BonusType.ARMOR_PENETRATION:
                _paramGroupRight.SetVisible(MarkerType.ARMOR_PENETRATION, true);
                break;
            case BonusType.MEDICINE:
                _paramGroupLeft.GetMarker(MarkerType.HP).SetValue(value);
                break;
            case BonusType.DEFENSE:
                if (delta > 0) AnimFlayText($"<sprite=3>+{delta}", FlyTextColor.GREEN);
                _paramGroupLeft.GetMarker(MarkerType.DEFENSE)?.SetValue(value);
                _paramGroupLeft.SetVisible(MarkerType.DEFENSE, value > 0);
                break;
            case BonusType.ACCURACY:
                _paramGroupRight.SetVisible(MarkerType.ACCURACY, true);
                break;
            case BonusType.REPAIR_KIT:
                _paramGroupLeft.GetMarker(MarkerType.HP).SetValue(value);
                break;
            case BonusType.AMMUNITION:
                if (delta > 0) AnimFlayText($"<sprite=2>+{delta}", FlyTextColor.GREEN);
                _paramGroupRight.GetMarker(MarkerType.ATTACK).SetValue(value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AnimationDealsDamage(ControllerType controllerType, Sound sound)
    {
        _animator.Play(controllerType == ControllerType.PLAYER ? "AttackPlayer" : "AttackEnemy");
        _audio.Play(sound);
    }

    private void RollBack()
    {
        _rectTransform.SetParent(_parent);
        if (_index == 0) _rectTransform.SetAsFirstSibling();
        else _rectTransform.SetAsLastSibling();
    }
    
    private void AnimBonusCard(Transform cell, Action callback)
    {
        CoverUp(false);

        transform.SetParent(cell);
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(90f, 120f);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(1f, 1f, 1f);

        StartCoroutine(AnimBonusCardDelay(callback));
    }

    private IEnumerator AnimBonusCardDelay(Action callback)
    {
        _animator.Play(ANIM_BONUS_DESTROY_NAME);
        var currentAnimationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.2f);
        //_audio.Play(Sound.USE_BONUS_CARD);
        yield return new WaitForSeconds(0.9f);
        callback?.Invoke();
        Destroyed();
    }

    private void AnimFail()
    {
        AnimFlayText("UI_FAIL", FlyTextColor.WHITE);
    }

    private void AnimFlayText(string text, FlyTextColor textColor)
    {
        var flyText = Instantiate(vfx, transform);
        flyText.Set(text, textColor);
    }

    private void MarkerHpUpdate(int hit, int hp)
    {
        AnimFlayText($"<sprite=0>-{hit}", FlyTextColor.RED);
        _paramGroupLeft.GetMarker(MarkerType.HP).SetValue(hp);
    }
    
    private void AnimRemoveDefense(int hit, int defense)
    {
        AnimFlayText($"<sprite=3>-{hit}", FlyTextColor.GRAY);
        _paramGroupLeft.GetMarker(MarkerType.DEFENSE)?.SetValue(defense);
        _paramGroupLeft.SetVisible(MarkerType.DEFENSE, defense > 0);
    }

    private void SetupExplosionEffect()
    {
        string vfxName;
        var pos = Vector2.zero;
        var position = transform.position;
        var sound = Sound.NONE;
                        
        switch (_unit.Type)
        {
            case UnitType.WARRIOR:
                vfxName = "Explosion_1";
                pos = transform.position;
                //sound = Sound.EXPLOSION_1;
                break;
            case UnitType.ARCHER:
                vfxName = "Explosion_2";
                pos = new Vector2(position.x, position.y + 20);
                //sound = Sound.EXPLOSION_2;
                break;
            case UnitType.MAGICIAN:
                vfxName = "Explosion_3";
                pos = new Vector2(position.x, position.y + 17);
                //sound = Sound.EXPLOSION_3;
                break;
            default:
                vfxName = "";
                break;
        }
        
        //Game.Instance.VfxController.Play(vfxName, pos);
        _audio.Play(sound);
    }
    
    private static IEnumerator Delay(Action callback1, Action callback2, float time = 0.73f)
    {
        callback1?.Invoke();
        yield return new WaitForSeconds(time);
        callback2?.Invoke();
    }
}