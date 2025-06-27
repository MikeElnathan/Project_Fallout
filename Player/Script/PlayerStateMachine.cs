using Godot;
using System.Collections.Generic;

public partial class PlayerStateMachine : BaseStateMachine
{
    private SignalBus signalBus;

    public override void _Ready()
    {
        signalBus = SignalBus.Instance;
        base._Ready();
    }

    protected override void GetAnimation()
    {
        animationPlayer = PlayerAnimation.Anim_Instance;
    }

    protected override void ReadSignal()
    {
        signalBus.Walk += () => changeState("Walk");
        signalBus.Run += () => changeState("Run");
        signalBus.Idle += () => changeState("Idle");
        signalBus.Jump += () => changeState("Jump");
    }
}
