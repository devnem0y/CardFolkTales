using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Data/DialogData", order = 30)] 
public class DialogData : ScriptableObject
{
    [SerializeField] private List<Speaker> _speakers;
    public List<Speaker> Speakers => _speakers;
}
