using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoseGroup : MonoBehaviour
{
    [SerializeField] private Button _btnRestart;
    [SerializeField] private Button _btnBackHome;

    public void RestartAddListener(UnityAction action)
    {
        _btnRestart.onClick.AddListener(action);
    }
    
    public void BackHomeAddListener(UnityAction action)
    {
        _btnBackHome.onClick.AddListener(action);
    }
}