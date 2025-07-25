using Godot;

public partial class SignalBus : Node3D
{
    //Sigleton instance
    private static SignalBus _instance;
    public static SignalBus Instance => _instance;
    private PlayerStateMachine playerStateMachine;
    private BlackBoard_Player playerBlackboard;

    public override void _Ready()
    {
        //return a single instance of this class unless it's not null
        if (_instance != null && _instance != this)
        {
            QueueFree();
            return;
        }
        _instance = this;

        playerBlackboard = BlackBoard_Player.Instance;
    }
    //Basic Player Movement
    [Signal] public delegate void WalkEventHandler();
    [Signal] public delegate void RunEventHandler();
    [Signal] public delegate void IdleEventHandler();
    [Signal] public delegate void JumpEventHandler();
    [Signal] public delegate void SleepEventHandler();
    [Signal] public delegate void SneakEventHandler();

    //Interaction

    public void EmitPlayerSignal(GlobalEnum.State action)
    {
        switch (action)
        {
            case GlobalEnum.State.Idle:
                EmitSignal(SignalName.Idle);
                playerBlackboard.SetStateInPlayerBlackboard(GlobalEnum.State.Idle);
                break;
            case GlobalEnum.State.Walk:
                EmitSignal(SignalName.Walk);
                playerBlackboard.SetStateInPlayerBlackboard(GlobalEnum.State.Walk);
                break;
            case GlobalEnum.State.Run:
                EmitSignal(SignalName.Run);
                playerBlackboard.SetStateInPlayerBlackboard(GlobalEnum.State.Run);
                break;
            case GlobalEnum.State.Jump:
                EmitSignal(SignalName.Jump);
                playerBlackboard.SetStateInPlayerBlackboard(GlobalEnum.State.Jump);
                break;
            case GlobalEnum.State.Sleep:
                EmitSignal(SignalName.Sleep);
                playerBlackboard.SetStateInPlayerBlackboard(GlobalEnum.State.Sleep);
                break;
            case GlobalEnum.State.Sneak:
                EmitSignal(SignalName.Sneak);
                playerBlackboard.SetStateInPlayerBlackboard(GlobalEnum.State.Sneak);
                break;
            default:
                GD.PrintErr("Unknown action type: ", action.ToString());
                break;

        }
    }
}
