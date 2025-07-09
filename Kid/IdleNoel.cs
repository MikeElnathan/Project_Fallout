using Godot;
using System;

public partial class IdleNoel : State
{
    public override void _Ready()
    {
        base._Ready();
    }
    public override void Enter()
    {
        GD.Print("Noel: Idle entered");
        base.Enter();
    }
    public override void Exit()
    {
        GD.Print("Noel: Idle exited");
        base.Exit();
    }

}
