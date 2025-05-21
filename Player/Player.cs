using Godot;
using System;

public partial class Player : CharacterBody3D
{
    private float gravity;
    private Vector3 _velocity;

    public override void _Ready()
    {
        //AsSingle() convert the result to float 
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        GD.Print("Player Script: Ready entered");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }


    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;

        if (!IsOnFloor())
        {
            _velocity.Y -= gravity;
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

}
