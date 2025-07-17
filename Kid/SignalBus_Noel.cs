using Godot;
using System;

public partial class SignalBus_Noel : Node3D
{
    private static SignalBus_Noel _instance_noel;
    public static SignalBus_Noel Instance_noel => _instance_noel;
    private BlackBoard_Player playerBlackboard;
    public SignalBus.ActionType curentPlayerState { get; private set; }
    [Signal]public delegate void PlayerStateSignalEventHandler();

    public override void _Ready()
    {
        if (_instance_noel != null && _instance_noel != this)
        {
            QueueFree();
            return;
        }
        _instance_noel = this;

        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
        receivedPlayerState();
    }
    public void receivedPlayerState()
    {
        playerBlackboard.Connect(BlackBoard_Player.SignalName.PlayerStateChanged, new Callable(this, nameof(setCurrentPlayerState)));
    }
    private void setCurrentPlayerState()
    {
        //comment
        curentPlayerState = playerBlackboard.currentState;
        EmitSignal(SignalName.PlayerStateSignal);
    }
}
