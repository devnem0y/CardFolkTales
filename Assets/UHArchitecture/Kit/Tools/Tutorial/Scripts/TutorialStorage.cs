using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStorage", menuName = "Storage/Tutorials", order = 10)]
public class TutorialStorage : ScriptableObject
{
    [SerializeField] private List<Tutorial> _tutorials;
    public List<Tutorial> Tutorials => _tutorials;
}