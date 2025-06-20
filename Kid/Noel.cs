using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    private CharacterBody3D Player;
    private Vector3 PlayerPosition;
    private float DistanceToPlayer;
    private float MaxDistance;
    private float Speed;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
        //Temporary value assignment. Later, to be manipulated by behavior tree
        MaxDistance = 5.0f;
        Speed = 100.0f;
    }
    private void FollowPlayer(double _delta)
    {
        if (DistanceToPlayer > MaxDistance)
        {
            Vector3 Direction = (PlayerPosition - GlobalPosition).Normalized();
            Velocity = Direction * Speed * (float)_delta;
        }
        else
        {
            Velocity = Vector3.Zero;
        }
    }
    public override void _Process(double delta)
    {
        //TODO
    }
    public override void _PhysicsProcess(double delta)
    {
        PlayerPosition = Player.GlobalPosition;
        DistanceToPlayer = GlobalPosition.DistanceTo(PlayerPosition);
        GD.Print("Distance to player: ", DistanceToPlayer);

        FollowPlayer(delta);
        MoveAndSlide();
    }

}
