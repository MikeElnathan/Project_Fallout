using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    private CharacterBody3D Player;
    private Vector3 PlayerPosition;
    private float DistanceToPlayer;
    private float MaxDistance;
    private float Speed;
    private bool followPlayer;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
        //Temporary value assignment. Later, to be manipulated by behavior tree
        MaxDistance = 3.0f;
        Speed = 200.0f;
    }
    private void CheckDistance()
    {
        if (DistanceToPlayer > MaxDistance)
        {
            followPlayer = true;
        }
        if (DistanceToPlayer < 2.0f)
        {
            followPlayer = false;
        }
    }
    private void FollowPlayer(double _delta)
    {
        Vector3 Direction = (PlayerPosition - GlobalPosition).Normalized();
        Velocity = Direction * Speed * (float)_delta;
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
        CheckDistance();

        if (followPlayer)
        {
            FollowPlayer(delta);
        }
        else
        {
            Velocity = Vector3.Zero;
        }

        MoveAndSlide();
    }

}
