using Godot;
using System;
using System.Threading.Tasks;

public partial class Walk_Noel : State
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
    public override void Enter()
    {
        GD.Print("Noel: walk state, velocity: ", noel.Velocity);
        //_ = EnterAsync();
    }
    private async Task EnterAsync()
    {
        noel.move = false;
        await noel.DelayReaction(noel.ReactionSpeed);
        noel.move = true;
    }
    public override void Exit()
    {
        //GD.Print("Noel: Follow player exited");
        base.Exit();
    }
}
