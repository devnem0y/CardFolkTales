using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UralHedgehog;

public abstract class Controller
{
    public bool IsLose { get; private set; }
    public bool IsAttacker { get; protected set; }
    
    public event Action<ControllerType> OnTurnDone;
    public event Action OnEndBattle;

    protected ControllerType _controllerType;
    protected Team _team;
    protected Counter _turnPoints;
    protected int _maxTurnPoints;
    protected Commander _commander;
    protected Deck _deckUnits;
    protected Deck _deckBonuses;

    /// <summary>
    /// Подготовка командира, назначение всех параметров
    /// </summary>
    public virtual void Preparation()
    {
        _commander.OnCardUse += CardUse;
        IsLose = false;
    }

    /// <summary>
    /// Старт битвы
    /// </summary>
    public void Ready(Action callback)
    {
        _commander.StartCoroutine(Refresh(callback));
    }

    /// <summary>
    /// Выдача карты
    /// </summary>
    public void IssueCard()
    {
        _commander.IssueCard(_controllerType, _turnPoints, _deckUnits, _deckBonuses);
    }

    /// <summary>
    /// Выполнить ход
    /// </summary>
    public virtual void Turn() { }

    public void UpdateStateCards()
    {
        _commander.UpdateStateCards(_controllerType, _turnPoints, _deckUnits, _deckBonuses);
    }

    public virtual void EndBattle()
    {
        _commander.OnCardUse -= CardUse;
        OnEndBattle?.Invoke();
    }

    public virtual void Win() { }

    public virtual void Lose() { }
    
    public void CheckCommander()
    {
        var hasHandUnit = _commander.HandUnits.CardCount > 0;
        var hasDeckUnit = _deckUnits.Count > 0;
        var hasFieldUnit = _controllerType == ControllerType.PLAYER ?
            _commander.Battlefield.PlayerFrontLine.HasUnits : 
            _commander.Battlefield.EnemyFrontLine.HasUnits;

        IsLose = !hasHandUnit && !hasDeckUnit && !hasFieldUnit;
    }
    
    protected internal void SetCommander(Commander commander)
    {
        _commander = commander;
    }
    
    protected void AddTurnPoints()
    {
        //TODO: Начисляем очки хода
        //_turnPoints = new Counter(_maxTurnPoints); // Вариант 1 = начислить очков по максимуму
        _turnPoints.Add(_maxTurnPoints); // Вариант 2 = начислить N очков
        _commander.LabelTurnPointsUpdate(_turnPoints);
    }
    
    protected void InvokerTurnDone()
    {
        OnTurnDone?.Invoke(_controllerType);
    }
    
    protected IEnumerator Battle(List<Cell> my, IReadOnlyCollection<Cell> opponent, float timeDelayStart,
        float timeDelayCallback, Action callback)
    {
        yield return new WaitForSeconds(timeDelayStart);

        foreach (var myCell in my)
        {
            if (myCell.Card == null) continue;

            //Пробегаемся по ячейкам оппонента и определяем таргет (Первая попавшаяся ячейка с картой)
            var target = (from opponentCell in opponent
                where opponentCell.Card != null
                select opponentCell.Card).FirstOrDefault();

            if (target != null) myCell.Card.Get<Unit>().DealsDamage(target.Get<Unit>(), _controllerType);

            yield return new WaitForSeconds(timeDelayCallback);
        }

        callback?.Invoke();
    }
    
    private void CardUse(CardBase cardBase)
    {
        _turnPoints.Withdraw(cardBase.TurnPoints);
        _commander.CardUse(_controllerType, _turnPoints, _deckUnits, _deckBonuses);
    }
    
    private IEnumerator Refresh(Action callback)
    {
        yield return new WaitUntil(() => _commander.Clear(_controllerType));
        _commander.LabelTurnPointsUpdate(_turnPoints);
        yield return new WaitForSeconds(1.85f);
        callback?.Invoke();
    }
}