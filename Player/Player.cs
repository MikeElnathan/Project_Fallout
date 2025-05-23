using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody3D
{
    private float speed = 10.0f;
    private float MESH_OFFSET = 90.0f;

    private Godot.Vector3 _velocity;
    private float jump_gravity;
    private float jump_velocity;
    private float fall_gravity;

    private float jump_height = 4.0f;
    private float time_to_peak = 0.5f;
    private float time_to_fall = 0.5f;

    private Node3D visual_mesh;
    private Camera3D camera;
    private Node3D CameraAndMesh;

    public override void _Ready()
    {
        MotionMode = CharacterBody3D.MotionModeEnum.Grounded;

        camera = GetNode<Camera3D>("CameraAndMesh/Camera_Arm/Camera3D");

        visual_mesh = GetNode<Node3D>("CameraAndMesh/Mesh");
        CameraAndMesh = GetNode<Node3D>("CameraAndMesh");

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
        Godot.Vector3 input_dir = Godot.Vector3.Zero;

        if (Input.IsActionPressed("Forward")) input_dir -= Godot.Vector3.Forward;
        if (Input.IsActionPressed("Backward")) input_dir -= Godot.Vector3.Back;
        if (Input.IsActionPressed("Left")) input_dir += Godot.Vector3.Left;
        if (Input.IsActionPressed("Right")) input_dir += Godot.Vector3.Right;

        if (input_dir != Godot.Vector3.Zero)
        {
            input_dir = input_dir.Normalized();
            Godot.Vector3 camera_forward = -camera.GlobalTransform.Basis.Z;
            Godot.Vector3 camera_right = camera.GlobalTransform.Basis.X;

            camera_forward.Y = 0.0f;
            camera_right.Y = 0.0f;
            camera_forward = camera_forward.Normalized();
            camera_right = camera_right.Normalized();

            Godot.Vector3 move_dir = (camera_forward * input_dir.Z + camera_right * input_dir.X).Normalized();
            _velocity = move_dir;
        }
        else
        {
            _velocity = Godot.Vector3.Zero;
        }

        Turn_player_relative_to_key();
    }

    private void Turn_player_relative_to_key()
    {
        if (Velocity.Length() > 0)
        {
            Godot.Vector3 move_dir = Velocity;
            move_dir.Y = 0.0f;
            if (move_dir.Length() > 0.1)
            {
                float target_angle = Mathf.Atan2(move_dir.X, move_dir.Z) + MESH_OFFSET;
                visual_mesh.Rotation = new Godot.Vector3(Rotation.X, Mathf.LerpAngle(Rotation.Y, target_angle, 0.15f), Rotation.Z);
            }
        }
    }

    private void Turn_player_relative_to_camera()
    {
        Godot.Vector3 camera_forward = camera.GlobalTransform.Basis.Z;
        camera_forward.Y = 0.0f;
        camera_forward = camera_forward.Normalized();

        Godot.Vector3 target_position = visual_mesh.GlobalTransform.Origin + camera_forward;
        target_position.Y = visual_mesh.GlobalPosition.Y;
        visual_mesh.LookAt(target_position, Godot.Vector3.Up);

        GD.Print("camera_forward: ", camera_forward, ", Rotation: ", Rotation, ", target_position: ", target_position);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            if (Input.IsActionJustPressed("Forward") || Input.IsActionJustPressed("Backward") || Input.IsActionJustPressed("Left") || Input.IsActionJustPressed("Right"))
            {
                Turn_player_relative_to_camera();
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
