using Godot;

public partial class StateMachineNoel : BaseStateMachine
{
    private BlackBoard_Player playerBlackboard;
    private CharacterBody3D noel;
    private Noel classNoel;
    private Vector3 playerPosition;
    private SignalBus_Noel signalBus_Noel;

    public override void _Ready()
    {
        base._Ready();
        playerBlackboard = BlackBoard_Player.Instance;
        signalBus_Noel = SignalBus_Noel.Instance_noel;
        noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
        classNoel = GetTree().GetFirstNodeInGroup("Noel") as Noel; //to access method inside Noel
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
