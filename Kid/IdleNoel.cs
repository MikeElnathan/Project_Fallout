using Godot;

public partial class IdleNoel : State
{
    private Noel noel;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
    }
    public override void Enter()
    {
        base.Enter();
        GD.Print("Idling");
    }
    public override void Exit()
    {
        base.Exit();
    }

}
