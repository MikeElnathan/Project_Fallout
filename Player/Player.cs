using Godot;
using System;

public partial class Player : CharacterBody3D
{
    private float speed = 10.0f;
    private float MESH_OFFSET = 0.0f;

    private Vector3 _velocity;
    private float jump_gravity;
    private float jump_velocity;
    private float fall_gravity;

    private float jump_height = 4.0f;
    private float time_to_peak = 0.5f;
    private float time_to_fall = 0.5f;

    private Node3D visual_mesh;

    public override void _Ready()
    {
        MotionMode = CharacterBody3D.MotionModeEnum.Grounded;
        visual_mesh = GetNode<Node3D>("Mesh");

        jump_velocity = (2.0f * jump_height) / time_to_peak;
        jump_gravity = (-2.0f * jump_height) / (float)Math.Pow(time_to_peak, 2);
        fall_gravity = (-2.0f * jump_height) / (float)Math.Pow(time_to_fall, 2);
    }

    private float Return_gravity()
    {
        return Velocity.Y > 0.0f ? jump_gravity : fall_gravity;
    }

    private void Jump()
    {
        _velocity.Y = jump_velocity;
    }

    private void Keyboard_move()
    {
        var input_dir = Input.GetVector("Left", "Right", "Forward", "Backward");
        Vector3 dir = (Transform.Basis * new Vector3(input_dir.X, 0, input_dir.Y)).Normalized();
        if (IsOnFloor())
        {
            _velocity = dir;
        }
    }

    private void Turn_player()
    {
        if (Velocity.Length() > 0.1)
        {
            Vector3 move_dir = Velocity;
            move_dir.Y = 0.0f;
            if (move_dir.Length() > 0.1)
            {
                float target_angle = (float)Math.Atan2(move_dir.X, move_dir.Z) + MESH_OFFSET;
                visual_mesh.Rotation = new Vector3(visual_mesh.Rotation.X, Mathf.LerpAngle(visual_mesh.Rotation.Y, target_angle, 0.3f), visual_mesh.Rotation.Z);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {

    }

    private void Handle_jump(double delta)
    {
        _velocity.Y += Return_gravity() * (float)delta;

        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
        {
            Jump();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Keyboard_move();
        Handle_jump(delta);

        Velocity = new Vector3(_velocity.X * speed, _velocity.Y, _velocity.Z * speed);

        Turn_player();
        MoveAndSlide();
    }

}
