using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tutorial
{
    [SerializeField] private string _name;
    [SerializeField] private List<TutorialStep> _steps;
    protected internal List<TutorialStep> Steps => _steps;
    
    public bool IsRun { get; private set; }
    public bool IsCompleted { get; private set; }

    public Tutorial(bool dataIsCompleted, List<TutorialStep> steps)
    {
        IsCompleted = dataIsCompleted;
        _steps = steps;
    }

    public void StepComplete(int index)
    {
        _steps[index].IsCompleted = true;
        
        StepRun(index + 1);

        IsCompleted = _steps[^1].IsCompleted;
    }

    public bool StepIsActual(int index)
    {
        return _steps[index].IsRun && !_steps[index].IsCompleted;
    }

    public bool StepIsCompleted(int index)
    {
        return _steps[index].IsCompleted;
    }

    public void StepRun(int index)
    {
        if (index >= _steps.Count) return;
        
        _steps[index].IsRun = true;

        IsRun = _steps[0].IsRun;
    }
}