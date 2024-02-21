using System;
using UralHedgehog;

public class BattleManager
{
    private readonly Controller _player;
    private readonly Controller _enemy;

    public event Action<ControllerType> OnTurn; 
    
    public BattleManager(Controller controller1, Controller controller2)
    {
        _player = controller1;
        _enemy = controller2;
        
        _player.Preparation();
        _enemy.Preparation();

        _player.OnTurnDone += BattleRound;
        _enemy.OnTurnDone += BattleRound;
        //Dispatcher.OnLeaveFight += PlayerLose;
    }

    public void Run()
    {
        _player.Ready(() =>
        {
            OnTurn?.Invoke(ControllerType.PLAYER);
            _player.IssueCard();
        });
        
        _enemy.Ready(() =>
        {
            OnTurn?.Invoke(ControllerType.AI);
            _enemy.IssueCard();
            if (_enemy.IsAttacker) _enemy.Turn();
        });
    }

    private void BattleRound(ControllerType controllerType)
    {
        switch (controllerType)
        {
            case ControllerType.PLAYER:
                _enemy.CheckCommander();

                if (_enemy.IsLose)
                {
                    _player.OnTurnDone -= BattleRound;
                    _enemy.OnTurnDone -= BattleRound;
                    //Dispatcher.OnLeaveFight -= PlayerLose;
                    _player.Win();
                    _enemy.EndBattle();
                }
                else
                {
                    OnTurn?.Invoke(ControllerType.AI);
                    _enemy.IssueCard();
                    _enemy.Turn();
                }
                break;
            case ControllerType.AI:
                _player.CheckCommander();

                if (_player.IsLose)
                {
                    PlayerLose();
                }
                else
                {
                    OnTurn?.Invoke(ControllerType.PLAYER);
                    _player.IssueCard();
                    _player.UpdateStateCards();
                    TutorialCheck();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
        }
    }

    private void PlayerLose()
    {
        _player.OnTurnDone -= BattleRound;
        _enemy.OnTurnDone -= BattleRound;
        //Dispatcher.OnLeaveFight -= PlayerLose;
        _player.Lose();
    }

    private void TutorialCheck()
    {
        /*if (Game.Instance.TutorialHandler.IsActualTutorialStep(0, 1))
        {
            Dispatcher.Send(Event.ON_TUTOR_BUTTON_END_TURN_LOCK, false);
            new Dialog(Game.Instance.TutorialsDialogsData[1]);
        }*/
    }
}