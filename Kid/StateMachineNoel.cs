using Godot;
using System;

public partial class StateMachineNoel : BaseStateMachine
{
    private SignalBus signalBus;
    private SignalBus_Noel signalBus_Noel;

    public override void _Ready()
    {
        signalBus = SignalBus.Instance;
        base._Ready();
    }
    protected override void ReadSignal()
    {
        signalBus.Walk += () => changeState("FollowPlayer");
        signalBus.Sleep += () => changeState("Sleep");
        signalBus.Sneak += () => changeState("Sneak");
    }

}
