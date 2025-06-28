using Godot;
using System;

public partial class FollowPlayer : State
{
    private CharacterBody3D noel;
    public override void _Ready()
    {
        base._Ready();
    }

    public override void Enter()
    {
        noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
        GD.Print("I found Noel: ", noel.GlobalPosition);    
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update(double delta)
    {
        base.Update(delta);
    }
    public override void PhysicUpdate(double delta)
    {
        base.PhysicUpdate(delta);
    }

    private void followPlayer()
    {
        //doSomething
    }
}
