using System.Collections.Generic;
using System.Linq;
using UralHedgehog;

public class TutorialHandler : ITutorialHandler
{
    internal List<Tutorial> _tutorials;
    
    private List<bool> _tutorialsData;
    private TutorialStorage _tutorialStorage;
    private List<DialogData> _dialogsData;
    
    public TutorialHandler(List<bool> tutorialsData, TutorialStorage tutorialStorage, List<DialogData> tutorialsDialogsData)
    {
        _tutorialsData = tutorialsData;
        _tutorialStorage = tutorialStorage;
        _dialogsData = tutorialsDialogsData;
        
        Create();
    }

    public void Create()
    {
        if (_tutorialsData.Count == 0)
        {
            _tutorialsData = _tutorialStorage.Tutorials.Select(tutorial => tutorial.IsCompleted).ToList();
        }
        
        _tutorials = new List<Tutorial>();

        for (var i = 0; i < _tutorialsData.Count; i++)
        {
            _tutorials.Add(new Tutorial(_tutorialsData[i], _tutorialStorage.Tutorials[i].Steps));
        }
    }

    public bool IsCompleted(int index)
    {
        return _tutorials[index].IsCompleted;
    }
    
    public bool IsCompletedTutorialStep(int tutorialIndex, int stepIndex)
    {
        return _tutorials[tutorialIndex].StepIsCompleted(stepIndex);
    }

    public bool IsActual(int index)
    {
        var tutor = _tutorials[index];
        return !tutor.IsCompleted && tutor.IsRun;
    }
    
    public bool IsActualTutorialStep(int tutorialIndex, int stepIndex)
    {
        var tutor = _tutorials[tutorialIndex];
        return tutor.StepIsActual(stepIndex);
    }

    public void Run(int index)
    {
        _tutorials[index].StepRun(0);
    }
    
    public void Complete(int tutorialIndex, int stepIndex)
    {
        _tutorials[tutorialIndex].StepComplete(stepIndex);
    }

    public void RunViaDialog(int index) // TODO: Костылек для запуска первого тутора
    {
        new Dialog(_dialogsData[0], () =>
        {
            _tutorials[index].StepRun(0);
        });
    }
}