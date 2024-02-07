using UnityEngine.Events;
using UralHedgehog;

public class Dialog
{
    private readonly DialogData _data;
    
    private int _currentSpeaker;
    private int _currentIndexMessage;
    private readonly UnityEvent _onEnd;
    
    /// <summary>
    /// Без события
    /// </summary>
    /// <param name="data">Диалог</param>
    public Dialog(DialogData data)
    {
        _data = data;
        _onEnd = new UnityEvent();
        Dispatcher.Send(EventD.ON_SHOW_DIALOG, this);
    }
    
    /// <summary>
    /// С событием по завершению диалога
    /// </summary>
    /// <param name="data">Диалог</param>
    /// <param name="callback">Что будет по завершинию диалога</param>
    public Dialog(DialogData data, UnityAction callback)
    {
        _data = data;
        _onEnd = new UnityEvent();
        _onEnd.AddListener(callback);
        Dispatcher.Send(EventD.ON_SHOW_DIALOG, this);
    }

    /// <summary>
    /// Вызываем в начале и по нажатию (пропустить / skip)
    /// </summary>
    /// <param name="sample">Шаблон сообщения</param>
    public void Talk(DSample sample)
    {
        if (_currentSpeaker > _data.Speakers.Count - 1)
        {
            _onEnd?.Invoke();
            return;
        }
        
        var speaker = _data.Speakers[_currentSpeaker];
        var message = speaker.Messages[_currentIndexMessage];
        
        sample.Init(speaker.Name, speaker.Avatar, speaker.Billet, message, speaker.Left);

        if (speaker.Messages.Count - 1 > _currentIndexMessage)
        {
            _currentIndexMessage++;
        }
        else
        {
            if (_data.Speakers.Count - 1 < _currentSpeaker) return;
            
            _currentSpeaker++; 
            _currentIndexMessage = 0;
        }
    }

    public void Send(UnityAction callback)
    {
        _onEnd?.AddListener(callback);
    }
}
