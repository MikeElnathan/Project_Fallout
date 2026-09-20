using Godot;

public partial class SignalBus : Node3D
{
    public enum ActionType
    {
        Idle, Walk, Run, Jump, Sleep, Sneak
    }
    private PlayerStateMachine playerStateMachine;
    private BlackBoard_Player playerBlackboard;

    public override void _Ready()
    {
        playerBlackboard = GetParent().GetChild(1) as BlackBoard_Player;
        GD.Print($"playerBlackboard: {playerBlackboard}");  
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
                playerBlackboard.SetStateInPlayerBlackboard(ActionType.Idle);
                
                break;
            case ActionType.Walk:
                EmitSignal(SignalName.Walk);
                playerBlackboard.SetStateInPlayerBlackboard(ActionType.Walk);
                break;
            case ActionType.Run:
                EmitSignal(SignalName.Run);
                playerBlackboard.SetStateInPlayerBlackboard(ActionType.Run);
                break;
            case ActionType.Jump:
                EmitSignal(SignalName.Jump);
                playerBlackboard.SetStateInPlayerBlackboard(ActionType.Jump);
                break;
            case ActionType.Sleep:
                EmitSignal(SignalName.Sleep);
                playerBlackboard.SetStateInPlayerBlackboard(ActionType.Sleep);
                break;
            case ActionType.Sneak:
                EmitSignal(SignalName.Sneak);
                playerBlackboard.SetStateInPlayerBlackboard(ActionType.Sneak);
                break;
            default:
                GD.PrintErr("Unknown action type: ", action.ToString());
                break;

        }
    }
}
