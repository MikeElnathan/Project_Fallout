using Godot;
using System;
using System.Threading.Tasks;

public partial class FollowPlayer : State
{
    private Noel noel;
    private CharacterBody3D player;
    private float disTreshold = 4.0f;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
    }
    public override async void Enter()
    {
        //GD.Print("Noel: Follow player state");
        noel.move = false;
        await noel.DelayReaction(noel.ReactionSpeed);
        followPlayer();
        base.Enter();
    }
    public override void Exit()
    {
        //GD.Print("Noel: Follow player exited");
        base.Exit();
    }
    public override void Update(double delta)
    {

    }
    public override void PhysicUpdate(double delta)
    {
        base.PhysicUpdate(delta);
    }
    private void followPlayer()
    {
        //check distance
        //currently target position in movementTargetPosition is empty. A different script will handle it, in MoodManager
        noel.move = true;
        //GD.Print("FollowPlayer set move to: ", noel.move);
    }
}
