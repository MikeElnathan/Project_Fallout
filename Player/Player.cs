using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody3D
{
    private float speed = 10.0f;
    private float MESH_OFFSET = 0.0f;

    private Godot.Vector3 _velocity;
    private float jump_gravity;
    private float jump_velocity;
    private float fall_gravity;

    private float jump_height = 4.0f;
    private float time_to_peak = 0.5f;
    private float time_to_fall = 0.5f;

    private Node3D visual_mesh;
    private Camera3D camera;

    public override void _Ready()
    {
        MotionMode = CharacterBody3D.MotionModeEnum.Grounded;

        camera = GetNode<Camera3D>("Camera_Arm/Camera3D");

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
        var dir = (Transform.Basis * new Godot.Vector3(input_dir.X, 0, input_dir.Y));
        if (IsOnFloor())
        {
            _velocity = dir;
        }
    }

    private void Turn_player()
    {
        Godot.Vector3 camera_forward = camera.GlobalTransform.Basis.Z;
        camera_forward.Y = 0.0f;
        camera_forward = camera_forward.Normalized();

        Godot.Vector3 target_position = GlobalTransform.Origin - camera_forward;
        GD.Print("target_position: ", target_position, ", camera_forward: ", camera_forward);

        LookAt(target_position, Godot.Vector3.Up);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            if (Input.IsActionPressed("Forward") || Input.IsActionPressed("Backward") || Input.IsActionPressed("Left") || Input.IsActionPressed("Right"))
            {
                Turn_player();
            }
        }
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

        Velocity = new Godot.Vector3(_velocity.X * speed, _velocity.Y, _velocity.Z * speed);

        MoveAndSlide();
    }

}
