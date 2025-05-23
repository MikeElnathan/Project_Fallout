using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody3D
{
    private float speed = 10.0f;
    private Godot.Vector3 move_dir = Godot.Vector3.Zero;
    private Godot.Vector2 input_dir = Godot.Vector2.Zero;

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

        camera = GetNode<Camera3D>("CameraAndMesh/Camera_Arm/Camera3D");

        visual_mesh = GetNode<Node3D>("CameraAndMesh/Mesh");

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
        input_dir = Input.GetVector("Left", "Right", "Forward", "Backward");

        if (input_dir.Length() == 0.0f)
        {  
            _velocity.X = 0.0f;
            _velocity.Z = 0.0f;
        }

        Godot.Vector3 forward = -camera.GlobalTransform.Basis.Z;
        Godot.Vector3 right = camera.GlobalTransform.Basis.X;

        forward.Y = 0.0f;
        right.Y = 0.0f;
        forward = forward.Normalized();
        right = right.Normalized();

        move_dir = (right * input_dir.X + forward * -input_dir.Y).Normalized();

        if (move_dir.Length() > 0.1f)
        {
            float target_angle = Mathf.Atan2(move_dir.X, move_dir.Z);

            float current_yaw = visual_mesh.Rotation.Y;
            float smoothed_angle = Mathf.LerpAngle(current_yaw, target_angle, 0.15f);

            visual_mesh.Rotation = new Godot.Vector3(0, smoothed_angle, 0);
        }

        _velocity.X = move_dir.X;
        _velocity.Z = move_dir.Z;

    }

    private void Handle_jump(double delta)
    {
        _velocity.Y += Return_gravity() * (float)delta;

        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
        {
            Jump();
        }
    }

    public void Signal_sender()
    {
        // send signals to state machine.
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            //Debug line
            //GD.Print($"input_dir: {input_dir}, move_dir: {move_dir}, _velocity: {_velocity}, Velocity: {Velocity}, IsOnFloor: {IsOnFloor()}");

        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Keyboard_move();
        Handle_jump(delta);

        if (IsOnFloor() && _velocity.Y < 0)
        {
            _velocity.Y = 0.0f;
        }

        Velocity = new Godot.Vector3(_velocity.X * speed, _velocity.Y, _velocity.Z * speed);

        MoveAndSlide();
    }

}
