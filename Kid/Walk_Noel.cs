using Godot;
using System;
using System.Threading.Tasks;

public partial class Walk_Noel : State
{
    private Noel _noel;
    private StateLabel _stateText;
    public override void _Ready()
    {
        base._Ready();
        _noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        _stateText = GetTree().GetFirstNodeInGroup("StateLabel") as StateLabel;
    }
    public override void Enter()
    {
        _stateText.noelStateText = "Walking";
    }
    public override void Exit()
    {
        base.Exit();
    }
}
