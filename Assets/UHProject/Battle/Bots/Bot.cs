using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UralHedgehog;
using Random = UnityEngine.Random;

public class Bot : Controller
{
    private readonly List<CardData> _cardDeck;
    private readonly CardStorage _cardStorage;

    private Coroutine _routineBattle;

    public Bot(BotData data, CardStorage cardStorage)
    {
        _cardStorage = cardStorage;
        _controllerType = ControllerType.AI;
        _cardDeck = new List<CardData>(data.CardDeck);

        IsAttacker = true;
    }

    public override void Preparation()
    {
        base.Preparation();
        _deckUnits = new Deck(_cardStorage, _cardDeck, CardType.UNIT);
        _deckBonuses = new Deck(_cardStorage, _cardDeck, CardType.BONUS);
        _maxTurnPoints = 7;
        _turnPoints = _maxTurnPoints;
        _commander.Init(_cardStorage);
    }

    public override void Turn()
    {
        _commander.StartCoroutine(Step());
    }

    public override void EndBattle()
    {
        base.EndBattle();
        _routineBattle = null;
    }

    private void SetTeam(Team playerTeam)
    {
        _team = playerTeam switch
        {
            Team.RED => Team.BLUE,
            Team.BLUE => Team.RED,
            Team.WHITE => Team.YELLOW,
            Team.YELLOW => Team.WHITE,
            _ => throw new ArgumentOutOfRangeException(nameof(playerTeam), playerTeam, null)
        };
    }

    private IEnumerator Step()
    {
        yield return new WaitForSeconds(0.35f);

        var isFirstStepTutor = false;

        if (Game.Instance.TutorialHandler.IsCompleted(0)) //TODO: Если туториал первой битвы пройден
        {
            var cardCells = _commander.Battlefield.EnemyFrontLine.CardCells();

            if (cardCells != null)
            {
                if (cardCells.Count is > 1 and < 3)
                {
                    var rnd = Random.Range(0, 11);
                    if (rnd <= 5)
                    {
                        if (ApplyBonus()) yield return new WaitForSeconds(2f);
                        if (ApplyUnit()) yield return new WaitForSeconds(1.3f);
                        ApplyBonus();
                    }
                    else
                    {
                        if (ApplyBonus()) yield return new WaitForSeconds(2f);
                        if (ApplyBonus()) yield return new WaitForSeconds(2f);
                    }
                }
                else
                {
                    if (ApplyUnit()) yield return new WaitForSeconds(1.3f);
                    if (ApplyBonus()) yield return new WaitForSeconds(2f);
                    ApplyBonus();
                }
            }
            else
            {
                if (ApplyUnit()) yield return new WaitForSeconds(1.3f);
                if (ApplyBonus()) yield return new WaitForSeconds(2f);
                if (ApplyUnit()) yield return new WaitForSeconds(1.3f);
                ApplyBonus();
            }
        }
        else //TODO: Заскриптованый ход (тутор)
        {
            if (!Game.Instance.TutorialHandler.IsCompletedTutorialStep(0, 0))
            {
                //TODO: Установка Война в ячейку
                ApplyUnit(UnitType.WARRIOR);
                isFirstStepTutor = true;
            }
            else
            {
                //TODO: Накидываем бонус лечения на Воина
                //TODO: Устанавливаем Лучника в ячейку
                ApplyBonus(BonusType.HEALTH);
                yield return new WaitForSeconds(2f);
                ApplyUnit(UnitType.ARCHER);
            }
        }

        if (_routineBattle != null) yield return null;
        yield return new WaitForSeconds(isFirstStepTutor ? 0f : 2f);
        _routineBattle = _commander.StartCoroutine(Battle(_commander.Battlefield.EnemyFrontLine.Cells,
            _commander.Battlefield.PlayerFrontLine.Cells, 0.6f, 1.3f,
            () =>
            {
                InvokerTurnDone();
                AddTurnPoints();
                if (_routineBattle == null) return;
                _commander.StopCoroutine(_routineBattle);
                if (isFirstStepTutor) Game.Instance.TutorialHandler.RunViaDialog(0); // TODO: Запустили тутор на битву
                _routineBattle = null;
            }));
    }

    private bool ApplyUnit()
    {
        if (_commander.HandUnits.CardCount <= 0) return false;

        var emptyCells = _commander.Battlefield.EnemyFrontLine.EmptyCells();
        var firstCard = _commander.HandUnits.GetFirstCard();
        Card secondCard = null;

        if (_commander.HandUnits.CardCount > 1)
        {
            secondCard = _commander.HandUnits.GetSecondCard();
        }

        Cell cell1 = null;
        Cell cell2 = null;

        if (emptyCells == null) return false;

        if (firstCard.TurnPoints <= _turnPoints)
        {
            cell1 = _commander.Battlefield.EnemyFrontLine.GetEmptyCellForUnit(firstCard.Get<Unit>().Type);
        }

        if (secondCard != null && secondCard.TurnPoints <= _turnPoints)
        {
            cell2 = _commander.Battlefield.EnemyFrontLine.GetEmptyCellForUnit(secondCard.Get<Unit>().Type);
        }

        Card cardUnit;

        if (cell1 != null && cell2 != null)
        {
            cardUnit = _commander.HandUnits.GetRandomCard();
        }
        else
        {
            cardUnit = cell1 != null ? _commander.HandUnits.GetFirstCard() : _commander.HandUnits.GetSecondCard();
        }

        if (cardUnit == null) return false;

        var cell = _commander.Battlefield.EnemyFrontLine.GetEmptyCellForUnit(cardUnit.Get<Unit>().Type);

        if (cell == null || cardUnit == null) return false;

        cell.SetupCard(cardUnit, ControllerType.AI);
        cardUnit.CoverUp(false);
        _turnPoints -= cardUnit.TurnPoints;
        return true;
    }

    private bool ApplyBonus()
    {
        var cardCells = _commander.Battlefield.EnemyFrontLine.CardCells();
        var allSignals = new List<BonusType>();

        if (cardCells == null) return false;

        foreach (var bonusType in cardCells
                     .Select(cardCell => cardCell != null ? cardCell.Card.Get<Unit>().NeedBonuses : null)
                     .Where(needBonuses => needBonuses is { Count: > 0 })
                     .SelectMany(needBonuses => needBonuses.Where(bonusType => !allSignals.Contains(bonusType))))
            allSignals.Add(bonusType);

        var cardBonuses = new List<Card>();

        foreach (var signal in allSignals)
        {
            var card = _commander.HandBonuses.GetCardWithType(signal);
            if (card == null) continue;
            cardBonuses.Add(card);
            break;
        }

        Card cardBonus = null;

        switch (cardBonuses.Count)
        {
            case 1:
                if (cardBonuses[0].TurnPoints <= _turnPoints)
                {
                    cardBonus = cardBonuses[0];
                }

                break;
            case 2:
            {
                var rnd = Random.Range(0, 11);
                if (rnd <= 5)
                {
                    if (cardBonuses[0].TurnPoints <= _turnPoints)
                    {
                        cardBonus = cardBonuses[0];
                    }
                    else if (cardBonuses[1].TurnPoints <= _turnPoints)
                    {
                        cardBonus = cardBonuses[1];
                    }
                }
                else
                {
                    if (cardBonuses[1].TurnPoints <= _turnPoints)
                    {
                        cardBonus = cardBonuses[1];
                    }
                }

                break;
            }
        }

        Cell cell = null;

        if (cardBonus != null) cell = _commander.Battlefield.EnemyFrontLine.GetCellsHasCardInUnitNeedBonus(cardBonus.Get<Bonus>().Type);
        if (cell == null) return false;

        _turnPoints -= cardBonus.TurnPoints;
        cardBonus.Use(ControllerType.AI, cell.transform, () => { cell.Card.Get<Unit>().SetupBonus(cardBonus); });

        return true;
    }

    private void ApplyUnit(UnitType unitType)
    {
        var cardUnit = _commander.HandUnits.GetCardWithType(unitType);
        if (cardUnit == null) return;

        var cell = _commander.Battlefield.EnemyFrontLine.GetEmptyCellForUnit(cardUnit.Get<Unit>().Type);

        if (cell == null || cardUnit == null) return;

        cell.SetupCard(cardUnit, ControllerType.AI);
        cardUnit.CoverUp(false);
        _turnPoints -= cardUnit.TurnPoints;
    }

    private void ApplyBonus(BonusType bonusType)
    {
        var cardBonus = _commander.HandBonuses.GetCardWithType(bonusType);;
        
        Cell cell = null;

        if (cardBonus != null) cell = _commander.Battlefield.EnemyFrontLine.GetCellsHasCardInUnitNeedBonus(cardBonus.Get<Bonus>().Type);
        if (cell == null) return;

        _turnPoints -= cardBonus.TurnPoints;
        cardBonus.Use(ControllerType.AI, cell.transform, () => { cell.Card.Get<Unit>().SetupBonus(cardBonus); });
    }
}