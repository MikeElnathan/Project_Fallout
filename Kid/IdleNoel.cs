using Godot;

public partial class IdleNoel : State
{
    private Noel _noel;
    private StateLabel _stateLabel;
    public override void _Ready()
    {
        base._Ready();
        _noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        _stateLabel = GetTree().GetFirstNodeInGroup("StateLabel") as StateLabel;
    }
    public override void Enter()
    {
        _stateLabel.noelStateText = "Idle";
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
