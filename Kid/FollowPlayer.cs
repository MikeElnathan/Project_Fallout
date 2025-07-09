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
        GD.Print("Noel: Follow player state");
        //reset nav agent
        //await ResetNavAgent();
        //simulate delayed reaction
        noel.reactiontimer = true;
        await noel.DelayReaction(noel.ReactionSpeed);
        noel.reactiontimer = false;
        followPlayer();
        base.Enter();
    }
    public override void Exit()
    {
        GD.Print("Noel: Follow player exited");
        base.Exit();
        //reset noel
        noel.ReactionSpeed = 0.01f;
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
        GD.Print("FollowPlayer set move to: ", noel.move);
    }
    private async Task ResetNavAgent()
    {
        //is this necessary?
        var navAgent = noel.GetNode<NavigationAgent3D>("NavigationAgent3D");

        navAgent.TargetPosition = noel.GlobalPosition;

        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }
}
