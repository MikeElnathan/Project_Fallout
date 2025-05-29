using Godot;
using System;

public partial class Player : CharacterBody3D
{
    private float speed;
    private float run_speed = 10.0f;
    private float walk_speed = 6.0f;
    private Godot.Vector3 move_dir = Godot.Vector3.Zero;
    private Godot.Vector2 input_dir = Godot.Vector2.Zero;
    private Godot.Vector3 target_velocity = Godot.Vector3.Zero;
    private float rotation_speed = 8.0f;
    private float move_damping = 25.0f;

    private float jump_gravity;
    private float jump_velocity;
    private float fall_gravity;

    private float jump_height = 2.0f;
    private float time_to_peak = 0.6f;
    private float time_to_fall = 0.4f;

    private Node3D visual_mesh;
    private Camera3D camera;

    public override void _Ready()
    {
        MotionMode = CharacterBody3D.MotionModeEnum.Grounded;

        camera = GetNode<Camera3D>("CameraAndMesh/Camera_Arm/Camera3D");

        visual_mesh = GetNode<Node3D>("CameraAndMesh/Mesh");

        speed = walk_speed;

        Jump_Physics();
    }
    private void Jump_Physics()
    {
        jump_velocity = (2.0f * jump_height) / time_to_peak;
        jump_gravity = (-2.0f * jump_height) / (float)Math.Pow(time_to_peak, 2);
        fall_gravity = (-2.0f * jump_height) / (float)Math.Pow(time_to_fall, 2);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
        {
            if (keyEvent.Pressed && Input.IsActionJustPressed("Run"))
            {
                speed = run_speed;
            }
            else if (!keyEvent.Pressed && Input.IsActionJustReleased("Run"))
            {
                speed = walk_speed;
            }

        }
    }
    private float return_gravity()
    {
        return Velocity.Y > 0.0f ? jump_gravity : fall_gravity;
    }
    private void jump()
    {
        Velocity = new Godot.Vector3(Velocity.X, jump_velocity, Velocity.Z);
    }
    private void rotate_Visual_Mesh()
    {
        if (move_dir.Length() > 0.1f)
        {
            float target_angle = Mathf.Atan2(move_dir.X, move_dir.Z);
            float current_yaw = visual_mesh.Rotation.Y;
            float smoothed_angle = Mathf.LerpAngle(current_yaw, target_angle, rotation_speed * (float)GetPhysicsProcessDeltaTime());

            visual_mesh.Rotation = new Godot.Vector3(0, smoothed_angle, 0);
        }
    }
    private void Keyboard_move()
    {
        input_dir = Input.GetVector("Left", "Right", "Forward", "Backward");

        Godot.Vector3 forward = -camera.GlobalTransform.Basis.Z;
        Godot.Vector3 right = camera.GlobalTransform.Basis.X;

        forward.Y = 0.0f;
        right.Y = 0.0f;
        forward = forward.Normalized();
        right = right.Normalized();

        if (input_dir.Length() > 0.1f)
        {
            move_dir = (right * input_dir.X + forward * -input_dir.Y).Normalized();
            target_velocity = move_dir * speed;
            rotate_Visual_Mesh();
        }
        else
        {
            target_velocity = Godot.Vector3.Zero;
        }
    }
    private void Handle_Movement(double delta)
    {
        Godot.Vector3 horizontal_velocity = new Godot.Vector3(Velocity.X, 0, Velocity.Z);

        if (target_velocity.Length() > 0)
        {
            horizontal_velocity = horizontal_velocity.MoveToward(target_velocity, move_damping * (float)delta);
        }
        else
        {
            horizontal_velocity = horizontal_velocity.MoveToward(Godot.Vector3.Zero, move_damping * (float)delta);
        }

        Velocity = new Godot.Vector3(horizontal_velocity.X, Velocity.Y, horizontal_velocity.Z);
    }
    private void Handle_jump(double delta)
    {
        Velocity = new Godot.Vector3(Velocity.X, Velocity.Y + return_gravity() * (float)delta, Velocity.Z);

        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
        {
            jump();
            Godot.Vector3 jump_momentum = move_dir * speed * 0.1f;
            Velocity = new Godot.Vector3(Velocity.X + jump_momentum.X, Velocity.Y, Velocity.Z + jump_momentum.Z);
        }

        if (IsOnFloor() && Velocity.Y < 0.0f)
        {
            Velocity = new Godot.Vector3(Velocity.X, 0.0f, Velocity.Z);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        Keyboard_move();
        Handle_Movement(delta);
        Handle_jump(delta);

        MoveAndSlide();
    }

}
