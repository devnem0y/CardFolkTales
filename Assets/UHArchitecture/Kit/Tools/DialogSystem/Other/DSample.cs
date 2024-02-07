using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;

public class DSample : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _avatar;
    [SerializeField] private Image _billet;
    [SerializeField] private RectTransform _window;
    [SerializeField] private TMP_Text _message;

    public void Init(string name, Sprite avatar, Sprite billet, string message, bool isLeft)
    {
        _name.text = Game.Instance.LocalizationManager.GetTranslate(name);
        var localizeName = _name.GetComponent<LocalizedTextMP>();
        localizeName.Key = name;
        _avatar.sprite = avatar;
        _billet.sprite = billet;
        _message.text = Game.Instance.LocalizationManager.GetTranslate(message);
        var localizeMessage = _message.GetComponent<LocalizedTextMP>();
        localizeMessage.Key = message;

        /*_window.localScale = new Vector3(isLeft ? 1 : -1, 1, 1);
        _avatar.rectTransform.localScale = new Vector3(isLeft ? 1 : -1, 1, 1);
        _avatar.rectTransform.anchoredPosition = new Vector2(isLeft ? -378.5f : 383.5f, 198.2f);
        _billet.rectTransform.anchoredPosition = new Vector2(isLeft ? -269f : 242f, 106.9f);*/
    }
}
