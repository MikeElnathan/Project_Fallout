using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    private CharacterBody3D Player;
    private Vector3 PlayerPosition;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
    }
    public override void _Process(double delta)
    {
        //TODO
        PlayerPosition = Player.GlobalPosition;
        GD.Print(PlayerPosition);
    }

    public override void _PhysicsProcess(double delta)
    {
    }

}
