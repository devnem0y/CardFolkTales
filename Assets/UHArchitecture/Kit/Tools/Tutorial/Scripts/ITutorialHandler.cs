public interface ITutorialHandler
{
    public bool IsCompleted(int index);
    public bool IsCompletedTutorialStep(int tutorialIndex, int stepIndex);
    public bool IsActual(int index);
    public bool IsActualTutorialStep(int tutorialIndex, int stepIndex);
    public void Run(int index);
    public void RunViaDialog(int index);
    public void Complete(int tutorialIndex, int stepIndex);
}