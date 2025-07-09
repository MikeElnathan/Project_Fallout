using Godot;
using System;

public partial class MoodManager : Node
{
    private Noel noel;
    private CharacterBody3D player;
    private Vector3 playerPosition;

    public override void _Ready()
    {
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        //player position to be obtained through player blackboard
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
        //temporary. to be manipulated based on mood
        noel.ReactionSpeed = 4.0f;
        noel.stoppingDistance = 2.0f;
    }
    public override void _Process(double delta)
    {
        noel.movementsTargetPosition = player.GlobalPosition;
        //to varied based on noel's mood
    }
}
