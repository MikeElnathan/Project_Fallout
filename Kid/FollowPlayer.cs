using Godot;
using System;

public partial class FollowPlayer : State
{
    private Noel noel;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
    }

    public override void Enter()
    {
        GD.Print("Noel: Follow player state");
        noel.followPlayer = true;
        base.Enter();
    }
    public override void Exit()
    {
        GD.Print("Noel: Follow player exited");
        noel.followPlayer = false;
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
