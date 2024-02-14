using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class PMenu : Widget
{
    [SerializeField] private Button _btnSettings;

    public override void Init()
    {
        _btnSettings.onClick.AddListener(() =>
        {
            Game.Instance.OpenViewSettings();
        });
    }
}
