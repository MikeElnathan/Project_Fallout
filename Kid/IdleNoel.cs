using Godot;
using System;

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
        GD.Print("Noel: Idle entered");
        base.Enter();
        noel.move = false;
        GD.Print("IdleNoel set noel.move to: ", noel.move);
    }
    public override void Exit()
    {
        GD.Print("Noel: Idle exited");
        base.Exit();
    }

}
