using Godot;

public partial class SignalBus : Node3D
{
    public enum ActionType
    {
        Idle, Walk, Run, Jump, Sleep, Sneak
    }
    //Sigleton instance
    private static SignalBus _instance;
    public static SignalBus Instance => _instance;
    private PlayerStateMachine playerStateMachine;

    public override void _Ready()
    {
        //return a single instance of this class unless it's not null
        if (_instance != null && _instance != this)
        {
            QueueFree();
            return;
        }
        _instance = this;
    }
    private void initPlayerStateMachine()
    {
        playerStateMachine = GetTree().GetFirstNodeInGroup("PlayerStateMachine") as PlayerStateMachine;
        GD.Print("PlayerStateMachine: ", playerStateMachine);
    }
    //Basic Player Movement
    [Signal] public delegate void WalkEventHandler();
    [Signal] public delegate void RunEventHandler();
    [Signal] public delegate void IdleEventHandler();
    [Signal] public delegate void JumpEventHandler();
    [Signal] public delegate void SleepEventHandler();
    [Signal] public delegate void SneakEventHandler();

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
                GD.Print("Walk emitted");
                break;
            case ActionType.Run:
                EmitSignal(SignalName.Run);
                GD.Print("Run emitted");
                break;
            case ActionType.Jump:
                EmitSignal(SignalName.Jump);
                break;
            case ActionType.Sleep:
                EmitSignal(SignalName.Sleep);
                break;
            case ActionType.Sneak:
                EmitSignal(SignalName.Sneak);
                break;
            default:
                GD.PrintErr("Unknown action type: ", action.ToString());
                break;

        }
    }
}
