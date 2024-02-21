using System.Linq;
using UnityEngine;
using UralHedgehog;

public class Battlefield : MonoBehaviour
{
    [SerializeField] private FrontLine _playerFrontLine;
    public FrontLine PlayerFrontLine => _playerFrontLine;
    [SerializeField] private FrontLine _enemyFrontLine;
    public FrontLine EnemyFrontLine => _enemyFrontLine;

    protected void Subscribe()
    {
        /*Dispatcher.OnCardSelected += CardSelected;
        Dispatcher.OnCardDeselect += CardDeselect;*/
    }

    protected void Unsubscribe()
    {
        /*Dispatcher.OnCardSelected -= CardSelected;
        Dispatcher.OnCardDeselect -= CardDeselect;*/
    }
    
    private void CardSelected(object arg)
    {
        if (arg is UnitType unitType)
        {
            foreach (var cell in _playerFrontLine.Cells)
            {
                cell.CheckValid(unitType);
            }
        }
        else
        {
            var bonusType = (BonusType) arg;

            foreach (var cell in _playerFrontLine.Cells.Where(cell => cell.Card != null))
            {
                cell.Card.Get<Unit>().CheckValid(bonusType);
            }
        }
    }

    private void CardDeselect()
    {
        foreach (var cell in _playerFrontLine.Cells)
        {
            cell.RemoveSelection();
        }
    }
}
