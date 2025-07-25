using Godot;
using System;
using System.Threading.Tasks;

public partial class Walk_Noel : State
{
    private Noel noel;
    private StateLabel stateText;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        stateText = GetTree().GetFirstNodeInGroup("StateLabel") as StateLabel;
    }
    public override void Enter()
    {
        stateText.noelStateText = "Walking";
    }
    public override void Exit()
    {
        base.Exit();
    }
}
