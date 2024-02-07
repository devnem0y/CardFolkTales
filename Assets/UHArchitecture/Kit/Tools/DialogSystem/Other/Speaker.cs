using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Speaker
{
    [SerializeField] private string _name;
    public string Name => _name;
    [SerializeField] private Sprite _avatar;
    public Sprite Avatar => _avatar;
    [SerializeField] private Sprite _billet;
    public Sprite Billet => _billet;
    [Space(3)]
    [Header("Отображение аватара")]
    [SerializeField] private bool _isLeft;
    public bool Left => _isLeft;
    [Space(3)]
    [SerializeField] [Multiline(6)] private List<string> _messages;
    public List<string> Messages => _messages;
}
