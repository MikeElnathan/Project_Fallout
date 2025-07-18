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
    public override async void Enter()
    {
        noel.move = false;
        await noel.DelayReaction(noel.ReactionSpeed);
        GD.Print("Noel: walk state");
        noel.move = true;
        base.Enter();
    }
    public override void Exit()
    {
        //GD.Print("Noel: Follow player exited");
        base.Exit();
    }
}
