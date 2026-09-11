using Godot;

public partial class IdleNoel : State
{
    private Noel noel;
    private StateLabel stateLabel;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        stateLabel = GetTree().GetFirstNodeInGroup("StateLabel") as StateLabel;
    }
    public override void Enter()
    {
        stateLabel.noelStateText = "Idle";
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

}
