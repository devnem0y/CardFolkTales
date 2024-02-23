using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UralHedgehog;

public class Unit
{
    public UnitType Type { get; }
    public int Hp { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public bool Accuracy { get; private set; }
    public bool ArmorPenetration { get; private set; }
    
    public bool IsValid { get; private set; }
    public List<BonusType> NeedBonuses { get; private set; }

    public event Action<BonusType, int, int> OnSetBonus;
    public event Action OnDestroyed;
    public event Action OnFail;
    public event Action<int, int> OnTakingDamage;
    public event Action<int, int> OnRemoveDefense;
    public event Action<ControllerType, Sound> OnDealsDamage;

    private readonly int _maxHp;
    private readonly CardBase _cardBase;
    
    public Unit(CardBase cardBase)
    {
        _cardBase = cardBase;
        
        Type = cardBase.UnitType;
        _maxHp = cardBase.Hp;
        Hp = _maxHp;
        Attack = cardBase.Attack;
        Defense = cardBase.Defense;
        
        CheckBonuses();
    }

    /// <summary>
    /// Наносим урон юниту
    /// </summary>
    /// <param name="unit">Юнит противника</param>
    public void DealsDamage(Unit unit, ControllerType controllerType)
    {
        var sound = Sound.NONE;

        switch ((Identity) _cardBase.Id)
        {
            case Identity.Stormtroopers:
            case Identity.Special_forces:
                //sound = Sound.FIRE_INFANTRY_1;
                break;
            case Identity.Motor_rifle:
                //sound = Sound.FIRE_INFANTRY_2;
                break;
            case Identity.BMP:
                //sound = Sound.FIRE_TANK_1;
                break;
            case Identity.Light_tank:
            case Identity.Heavy_tank:
                //sound = Sound.FIRE_TANK_2;
                break;
            case Identity.Mortar:
                //sound = Sound.FIRE_ARTILLERY_1;
                break;
            case Identity.Barrel_artillery:
                //sound = Sound.FIRE_ARTILLERY_2;
                break;
            case Identity.MLRS:
                //sound = Sound.FIRE_ARTILLERY_3;
                break;
        }
        
        OnDealsDamage?.Invoke(controllerType, sound);
        
        if (Type == UnitType.WARRIOR && unit.Type != UnitType.WARRIOR)
        {
            if (ArmorPenetration) unit.DamageCalculation(Attack, Accuracy);
            else OnFail?.Invoke();
        }
        else
        {
            unit.DamageCalculation(Attack, Accuracy);
        }
    }

    /// <summary>
    /// Просчет получаемого урона
    /// </summary>
    /// <param name="damage">Урон</param>
    /// <param name="accuracyOpponent">Меткость</param>
    private void DamageCalculation(int damage, bool accuracyOpponent)
    {
        if (accuracyOpponent)
        {
            TakingDamage(damage);
        }
        else
        {
            if (Defense > 0)
            {
                if (damage >= Defense)
                {
                    var remainingDamage = damage - Defense;
                    var delta = Defense;
                    Defense = 0;
                    OnRemoveDefense?.Invoke(delta, Defense);

                    if (remainingDamage > 0) TakingDamage(remainingDamage, true);
                }
                else
                {
                    Defense -= damage;
                    OnRemoveDefense?.Invoke(damage, Defense);
                }
            }
            else
            {
                TakingDamage(damage);
            }
        }

        if (Hp < _maxHp && Hp != 0)
        {
            NeedBonuses.Add( Type == UnitType.WARRIOR ? BonusType.MEDICINE : BonusType.REPAIR_KIT);
        }
        
        if (Hp != 0) return;
        
        //TODO: Уничтожить карту и удалить из ячейки на поле битвы
        OnDestroyed?.Invoke();
    }

    /// <summary>
    /// Получение урона
    /// </summary>
    /// <param name="damage">Урон</param>
    /// <param name="isDelay"></param>
    private void TakingDamage(int damage, bool isDelay = false)
    {
        var hit = damage > Hp ? Hp : damage;
        
        if (Hp <= damage) Hp = 0;
        else Hp -= damage;

        if (isDelay)
        {
            Game.Instance.StartCoroutine(AnimDelay(0.42f, () =>
            {
                OnTakingDamage?.Invoke(hit, Hp);
            }));
        }
        else
        {
            OnTakingDamage?.Invoke(hit, Hp);
        }
    }

    public void SetupBonus(Card cardBonus, AudioComponent audio = null)
    {
        var bonus = cardBonus.Get<Bonus>();
        var value = 0;
        
        switch (bonus.Type)
        {
            case BonusType.AMMUNITION:
                Attack += bonus.Magnitude;
                value = Attack;
                break;
            case BonusType.MEDICINE:
                Hp = _maxHp;
                value = Hp;
                NeedBonuses.Remove(BonusType.MEDICINE);
                break;
            case BonusType.REPAIR_KIT:
                Hp = _maxHp;
                value = Hp;
                NeedBonuses.Remove(BonusType.REPAIR_KIT);
                break;
            case BonusType.DEFENSE:
                Defense += bonus.Magnitude;
                value = Defense;
                if (Game.Instance.TutorialHandler.IsActualTutorialStep(0, 2))
                {
                    Game.Instance.TutorialHandler.Complete(0, 2);
                    Dispatcher.Send(EventD.ON_TUTOR_BUTTON_END_TURN_LOCK, true);
                }
                break;
            case BonusType.ACCURACY:
                Accuracy = true;
                NeedBonuses.Remove(BonusType.ACCURACY);
                break;
            case BonusType.ARMOR_PENETRATION:
                ArmorPenetration = true;
                NeedBonuses.Remove(BonusType.ARMOR_PENETRATION);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        OnSetBonus?.Invoke(bonus.Type, value, bonus.Magnitude);
        //if (audio != null) audio.Play(Sound.USE_BONUS_CARD);
        cardBonus.Use(ControllerType.PLAYER);
    }
    
    public void CheckValid(BonusType bonusType)
    {
        switch (bonusType)
        {
            case BonusType.ARMOR_PENETRATION:
                IsValid = Type == UnitType.WARRIOR && !ArmorPenetration;
                break;
            case BonusType.MEDICINE:
                IsValid = Type == UnitType.WARRIOR && Hp < _maxHp;
                break;
            case BonusType.ACCURACY:
                IsValid = !Accuracy;
                break;
            case BonusType.REPAIR_KIT:
                IsValid = Type != UnitType.WARRIOR && Hp < _maxHp;
                break;
            case BonusType.AMMUNITION:
            case BonusType.DEFENSE:
            default:
                IsValid = true;
                break;
        }

        if (IsValid)
        {
            //TODO: Валидное выделение
        }
        else
        {
            //TODO: Залоченое выделение
        }
    }

    protected internal void CheckBonuses()
    {
        NeedBonuses = new List<BonusType>
        {
            BonusType.ACCURACY,
            BonusType.AMMUNITION,
            BonusType.DEFENSE
        };
        
        if (Type == UnitType.WARRIOR) NeedBonuses.Add(BonusType.ARMOR_PENETRATION);
    }
    
    private static IEnumerator AnimDelay(float timeDelay, Action callback)
    {
        yield return new WaitForSeconds(timeDelay);
        callback?.Invoke();
    }
}
