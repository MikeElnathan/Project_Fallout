using Godot;
using System;

public partial class Player : CharacterBody3D
{
    //--- Move Variables ---
    private float _speed;
    private float _runSpeed = 8.0f;
    private float _walkSpeed = 2.0f;
    private bool _run = false;
    private bool _jump = false;
    private Godot.Vector3 _moveDir = Godot.Vector3.Zero;
    private Godot.Vector2 _inputDir = Godot.Vector2.Zero;
    private Godot.Vector3 _targetVelocity = Godot.Vector3.Zero;
    private float _rotationSpeed = 8.0f;
    private float _moveDamping = 25.0f;

    //--- Jump Variables ---
    private float _jumpHeight = 2.0f;
    private float _timeToPeak = 0.6f;
    private float _timeToFall = 0.4f;
    private float _jumpGravity;
    private float _jumpVelocity;
    private float _fallGravity;

    // --- Scene references ---
    private Node3D _visualMesh;
    private Camera3D _camera;

    // --- State Handling ---
    private GlobalEnum.State _currentAction;
    private GlobalEnum.State _newAction;
    private SignalBus _signalBus;

    public override void _Ready()
    {
        MotionMode = CharacterBody3D.MotionModeEnum.Grounded;

        _camera = GetNode<Camera3D>("CameraAndMesh/Camera_Arm/Camera3D");

        _visualMesh = GetNode<Node3D>("CameraAndMesh/Mesh");

        _speed = _walkSpeed;

        _signalBus = SignalBus.Instance;
        _signalBus.EmitPlayerSignal(GlobalEnum.State.Idle);

        Jump_Physics();
    }
    public override void _PhysicsProcess(double delta)
    {
        Keyboard_move();
        Handle_Movement(delta);
        Handle_jump(delta);
        State_Signal_Sender();

        MoveAndSlide();
    }

    /// <summary>Applies gravity, handles jump input, and manages landing.</summary>
    private void Jump_Physics()
    {
        _jumpVelocity = (2.0f * _jumpHeight) / _timeToPeak;
        _jumpGravity = (-2.0f * _jumpHeight) / Mathf.Pow(_timeToPeak, 2);
        _fallGravity = (-2.0f * _jumpHeight) / Mathf.Pow(_timeToFall, 2);
    }
    private float return_gravity()
    {
        return Velocity.Y > 0.0f ? _jumpGravity : _fallGravity;
    }
    private void jump()
    {
        Velocity = new Godot.Vector3(Velocity.X, _jumpVelocity, Velocity.Z);
    }
    private void Handle_jump(double delta)
    {
        Velocity = new Godot.Vector3(Velocity.X, Velocity.Y + return_gravity() * (float)delta, Velocity.Z);

        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
        {
            jump();
            _jump = true;
            Godot.Vector3 jump_momentum = _moveDir * _speed * 0.1f;
            Velocity = new Godot.Vector3(Velocity.X + jump_momentum.X, Velocity.Y, Velocity.Z + jump_momentum.Z);
        }

        if (IsOnFloor() && Velocity.Y < 0.0f)
        {
            _jump = false;
            Velocity = new Godot.Vector3(Velocity.X, 0.0f, Velocity.Z);
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
        {
            if (keyEvent.Pressed && Input.IsActionJustPressed("Run"))
            {
                _speed = _runSpeed;
                _run = true;
            }
            else if (!keyEvent.Pressed && Input.IsActionJustReleased("Run"))
            {
                _speed = _walkSpeed;
                _run = false;
            }
        }
    }
    private void rotate__visualMesh()
    {
        if (_moveDir.Length() > 0.1f)
        {
            float target_angle = Mathf.Atan2(_moveDir.X, _moveDir.Z);
            float current_yaw = _visualMesh.Rotation.Y;
            float smoothed_angle = Mathf.LerpAngle(current_yaw, target_angle, _rotationSpeed * (float)GetPhysicsProcessDeltaTime());

            _visualMesh.Rotation = new Godot.Vector3(0, smoothed_angle, 0);
        }
    }
    private void Keyboard_move()
    {
        _inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");

        Godot.Vector3 forward = -_camera.GlobalTransform.Basis.Z;
        Godot.Vector3 right = _camera.GlobalTransform.Basis.X;

        forward.Y = 0.0f;
        right.Y = 0.0f;
        forward = forward.Normalized();
        right = right.Normalized();

        if (_inputDir.Length() > 0.1f)
        {
            _moveDir = (right * _inputDir.X + forward * -_inputDir.Y).Normalized();
            _targetVelocity = _moveDir * _speed;
            rotate__visualMesh();
        }
        else
        {
            _targetVelocity = Godot.Vector3.Zero;
        }
    }
    private void Handle_Movement(double delta)
    {
        Godot.Vector3 horizontal_velocity = new Godot.Vector3(Velocity.X, 0, Velocity.Z);

        if (_targetVelocity.Length() > 0)
        {
            horizontal_velocity = horizontal_velocity.MoveToward(_targetVelocity, _moveDamping * (float)delta);
        }
        else
        {
            horizontal_velocity = horizontal_velocity.MoveToward(Godot.Vector3.Zero, _moveDamping * (float)delta);
        }
        Velocity = new Godot.Vector3(horizontal_velocity.X, Velocity.Y, horizontal_velocity.Z);
    }
    private void State_Signal_Sender()
    {
        if (_targetVelocity.Length() > 0)
        {
            _newAction = _run ? GlobalEnum.State.Run : GlobalEnum.State.Walk;
        }
        else
        {
            _newAction = GlobalEnum.State.Idle;
        }

        if (_jump)
        {
            _newAction = GlobalEnum.State.Jump;
        }

        if (_newAction != _currentAction)
        {
            _signalBus.EmitPlayerSignal(_newAction);
            _currentAction = _newAction;
        }
    }

}
