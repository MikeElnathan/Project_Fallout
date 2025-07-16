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

        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
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
                playerBlackboard.SetStateInPlayerBlackboard(BlackBoard_Player.PlayerState.Idle);
                break;
            case ActionType.Walk:
                EmitSignal(SignalName.Walk);
                playerBlackboard.SetStateInPlayerBlackboard(BlackBoard_Player.PlayerState.Walk);
                break;
            case ActionType.Run:
                EmitSignal(SignalName.Run);
                playerBlackboard.SetStateInPlayerBlackboard(BlackBoard_Player.PlayerState.Run);
                break;
            case ActionType.Jump:
                EmitSignal(SignalName.Jump);
                playerBlackboard.SetStateInPlayerBlackboard(BlackBoard_Player.PlayerState.Jump);
                break;
            case ActionType.Sleep:
                EmitSignal(SignalName.Sleep);
                playerBlackboard.SetStateInPlayerBlackboard(BlackBoard_Player.PlayerState.Sleep);
                break;
            case ActionType.Sneak:
                EmitSignal(SignalName.Sneak);
                playerBlackboard.SetStateInPlayerBlackboard(BlackBoard_Player.PlayerState.Sneak);
                break;
            default:
                GD.PrintErr("Unknown action type: ", action.ToString());
                break;

        }
    }
}
