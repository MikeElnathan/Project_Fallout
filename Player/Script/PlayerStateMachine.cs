using Godot;

public partial class PlayerStateMachine : BaseStateMachine
{
    private SignalBus _signalBus;

    public override void _Ready()
    {
        _signalBus = SignalBus.Instance;
        base._Ready();
    }
    protected override void GetAnimation()
    {
        animationPlayer = PlayerAnimation.Anim_Instance;
    }
    protected override void ReadSignal()
    {
        base.ReadSignal();
        _signalBus.Connect(SignalBus.SignalName.Idle, new Callable(this, nameof(onIdle)));
        _signalBus.Connect(SignalBus.SignalName.Walk, new Callable(this, nameof(onWalk)));
        _signalBus.Connect(SignalBus.SignalName.Jump, new Callable(this, nameof(onJump)));
        _signalBus.Connect(SignalBus.SignalName.Run, new Callable(this, nameof(onRun)));
        _signalBus.Connect(SignalBus.SignalName.Sleep, new Callable(this, nameof(onSleep)));
        _signalBus.Connect(SignalBus.SignalName.Sneak, new Callable(this, nameof(onSneak)));
    }
    private void onIdle() => changeState("Idle");
    private void onWalk() => changeState("Walk");
    private void onJump() => changeState("Jump");
    private void onRun() => changeState("Run");
    private void onSleep() => changeState("Sleep");
    private void onSneak() => changeState("Sneak");
}
