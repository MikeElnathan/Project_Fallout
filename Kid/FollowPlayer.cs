using Godot;
using System;

public partial class FollowPlayer : State
{
    private Noel noel;
    private CharacterBody3D player;
    private float disTreshold = 5.0f;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
    }

    public override void Enter()
    {
        GD.Print("Noel: Follow player state");
        followPlayer();
        base.Enter();
    }
    public override void Exit()
    {
        GD.Print("Noel: Follow player exited");
        base.Exit();
    }
    public override void Update(double delta)
    {
        followPlayer();
    }
    public override void PhysicUpdate(double delta)
    {
        base.PhysicUpdate(delta);
    }

    private void followPlayer()
    {
        //check distance
        float distance = noel.GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distance > disTreshold)
        {
            noel.followPlayer = true;
        }
        else
        {
            noel.followPlayer = false;
        }
    }
}
