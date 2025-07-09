using Godot;
using System;

public partial class MoodManager : Node
{
    private Noel noel;
    private CharacterBody3D player;

    public override void _Ready()
    {
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
    }
    public override void _Process(double delta)
    {
        //temporary set-up
        noel.movementsTargetPosition = player.GlobalPosition;
    }
}
