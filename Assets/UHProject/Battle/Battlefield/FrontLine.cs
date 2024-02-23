using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FrontLine : MonoBehaviour
{
    [SerializeField] private List<Cell> _cells;
    public List<Cell> Cells => _cells;

    /// <summary>
    /// Возвращает true если есть юниты в ячейках
    /// </summary>
    public bool HasUnits => CheckUnits();

    /// <summary>
    /// Возвращает список ячеек с картами
    /// </summary>
    public List<Cell> CardCells()
    {
        var cells = _cells.Where(cell => !cell.IsEmpty).ToList();
        return cells.Count > 0 ? cells : null;
    }
    
    /// <summary>
    /// Возвращает список с пустыми ячейками
    /// </summary>
    public List<Cell> EmptyCells()
    {
        var cells = _cells.Where(cell => cell.IsEmpty).ToList();
        return cells.Count > 0 ? cells : null;
    }
    
    /// <summary>
    /// Возвращает пустую ячейку данного типа
    /// </summary>
    public Cell EmptyCell(CellType cellType)
    {
        return _cells.FirstOrDefault(cell => cell.IsEmpty && cell.Type == cellType);
    }

    /// <summary>
    /// Возвращает первопопавшуюся ячейку, которая содержит в себе юнита с данным типом 
    /// </summary>
    public Cell GetCellHasCardInUnit(UnitType unitType)
    {
        return _cells.Where(cell => cell.Card != null).
            FirstOrDefault(cell => cell.Card.Get<Unit>().Type == unitType);
    }
    
    /// <summary>
    /// Возвращает список ячеек, которые содержат в себе юнита данного типа
    /// </summary>
    public List<Cell> CellsHasCardInUnit(UnitType unitType)
    {
        var cells = _cells.Where(cell => cell.Card != null).
            Where(cell => cell.Card.Get<Unit>().Type == unitType).ToList();

        return cells.Count > 0 ? cells : null;
    }
    
    /// <summary>
    /// Возвращает случайную ячейку, которая содержит в себе юнита данного типа
    /// </summary>
    public Cell GetRandomCellHasCardInUnit(UnitType unitType)
    {
        var cells = CellsHasCardInUnit(unitType);
        
        if (cells.Count < 2) return cells[0];

        var rnd = Random.Range(0, 11);
        return rnd <= 5 ? cells[0] : cells[1];
    }

    /// <summary>
    /// Возвращает пустую ячейку под нужный тип юнита
    /// </summary>
    public Cell GetEmptyCellForUnit(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.WARRIOR:
                var cell = EmptyCell(CellType.WA);
                if (cell != null) return cell;
                break;
            case UnitType.ARCHER:
                var cellWA = EmptyCell(CellType.WA);
                var cellAM = EmptyCell(CellType.AM);
                if (cellWA == null || cellAM == null) return cellWA != null ? cellWA : cellAM;
                var rnd = Random.Range(0, 11);
                return rnd <= 5 ? cellWA : cellAM;
            case UnitType.MAGICIAN:
                var cellAMM = EmptyCell(CellType.AM);
                var cellM = EmptyCell(CellType.M);
                if (cellM == null || cellAMM == null) return cellM != null ? cellM : cellAMM;
                var rnd1 = Random.Range(0, 11);
                return rnd1 <= 5 ? cellM : cellAMM;
            default:
                throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null);
        }

        return null;
    }

    /// <summary>
    /// Возвращает ячейку с юнетом на которую можно наложить бонус данного типа
    /// </summary>
    public Cell GetCellsHasCardInUnitNeedBonus(BonusType bonusType)
    {
        return (from cell in CardCells() let unit = cell.Card.Get<Unit>() 
            where unit.NeedBonuses.Contains(bonusType) select cell).FirstOrDefault();
    }

    private bool CheckUnits()
    {
        return Cells.Any(cell => cell.Card != null && !cell.IsEmpty);
    }
}