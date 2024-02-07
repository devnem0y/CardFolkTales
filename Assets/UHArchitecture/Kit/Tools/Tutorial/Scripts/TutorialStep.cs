using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    [SerializeField] private string _name;
    
    public bool IsRun { get; set; }
    public bool IsCompleted { get; set; }
}