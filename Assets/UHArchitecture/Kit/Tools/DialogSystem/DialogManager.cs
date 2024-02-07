using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private Button _button;
    [SerializeField] private DSample _sample;

    private Dialog _dialog;

    private void Awake()
    {
        Dispatcher.OnShowDialog += OnShow;
        _button.onClick.AddListener(OnSkip);
    }

    private void OnDestroy()
    {
        Dispatcher.OnShowDialog -= OnShow;
    }

    private void OnShow(object arg)
    {
        _dialog = (Dialog) arg;
        _dialog.Send(OnHide);
        _dialog.Talk(_sample);
        _wrapper.SetActive(true);
    }

    private void OnHide()
    {
        _wrapper.SetActive(false);
    }

    private void OnSkip()
    {
        _dialog.Talk(_sample);
    }
}
