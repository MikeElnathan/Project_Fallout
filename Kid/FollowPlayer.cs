using Godot;
using System;

public partial class FollowPlayer : State
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void Enter()
    {
        GD.Print("Noel: Follow player state");
        base.Enter();
    }
    public override void Exit()
    {
        GD.Print("Noel: Follow player exited");
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
