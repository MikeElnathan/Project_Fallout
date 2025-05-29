using Godot;
using System;
using System.Collections;

public partial class SignalBus : Node3D
{
    public enum ActionType
    {
        Idle, Walk, Run, Jump
    }
    //Sigleton instance
    private static SignalBus _instance;
    public static SignalBus Instance => _instance;

    public override void _Ready()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    //Basic Player Movement
    [Signal] public delegate void WalkEventHandler();
    [Signal] public delegate void RunEventHandler();
    [Signal] public delegate void IdleEventHandler();
    [Signal] public delegate void JumpEventHandler();

    //Interaction

    public void EmitPlayerSignal(ActionType action)
    {
        switch (action)
        {
            case ActionType.Idle:
                EmitSignal(SignalName.Idle);
                break;
            case ActionType.Walk:
                EmitSignal(SignalName.Walk);
                break;
            case ActionType.Run:
                EmitSignal(SignalName.Run);
                break;
            case ActionType.Jump:
                EmitSignal(SignalName.Jump);
                break;
            default:
                GD.PrintErr("Unknown action type: ", action.ToString());
                break;

        }
    }
}
