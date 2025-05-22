using Godot;
using System;

public partial class Player : CharacterBody3D
{
    private float speed = 10.0f;
    private Vector3 _velocity;
    private float jump_gravity;
    private float jump_velocity;
    private float fall_gravity;

    private float jump_height = 0.0f;
    private float time_to_peak = 0.0f;
    private float time_to_fall = 0.0f;


    public override void _Ready()
    {
        MotionMode = CharacterBody3D.MotionModeEnum.Grounded;

        jump_velocity = (2.0f * jump_height) / time_to_peak;
        jump_gravity = (-2.0f * jump_height) / (float)Math.Pow(time_to_peak, 2);
        fall_gravity = (-2.0f * jump_height) / (float)Math.Pow(time_to_fall, 2);
    }

    private float Return_gravity()
    {
        return Velocity.Y > 0.0f ? jump_gravity : fall_gravity;
    }

    public override void _Input(InputEvent @event)
    {

    }


    public override void _PhysicsProcess(double delta)
    {

        MoveAndSlide();
    }

}
