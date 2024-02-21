using System;

public abstract class TestAbstractController
{
    public bool IsLose { get; private set; }
    
    public event Action OnEndBattle;
    
    protected int _maxTurnPoints;
    
    /// <summary>
    /// Подготовка командира, назначение всех параметров
    /// </summary>
    public virtual void Preparation()
    {
        IsLose = false;
    }
    
    /// <summary>
    /// Выдача карты
    /// </summary>
    public void IssueCard()
    {
        _maxTurnPoints = 0;
    }

    /// <summary>
    /// Выполнить ход
    /// </summary>
    public virtual void Turn() { }
}