using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CameraArm : Node3D
{
    private float _orbitSensitivity = 6.5f;

    private float _orbitVelocity = 0.0f;
    private float _verticalVelocity = 0.0f;
    private float _currentYaw = 0.0f;
    private float _currentPitch = 0.0f;
    private float _minPitch = 0.0f;
    private float _maxPitch = 25.0f;
    private float _deadZone = 1.5f;

    private float _zoom = 45.0f;
    private float _zoomValue = 2.0f;
    private float _zoomMin = 30.0f;
    private float _zoomMax = 75.0f;
    private float _targetZoom;

    private Camera3D _camera;

    public override void _Ready()
    {
        _currentYaw = RotationDegrees.Y;
        _camera = GetChild<Camera3D>(0);
        _camera.Fov = _zoom;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (Input.IsActionPressed("Orbit_Camera"))
            {
                if (Math.Abs(mouseMotion.Relative.X) > _deadZone)
                {
                    _orbitVelocity -= mouseMotion.Relative.X * _orbitSensitivity;
                }
                if (Math.Abs(mouseMotion.Relative.Y) > _deadZone)
                {
                    _verticalVelocity -= mouseMotion.Relative.Y * _orbitSensitivity;
                }
            }
        }
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.WheelUp)
            {
                _zoom -= _zoomValue;
            }
            if (mouseButton.ButtonIndex == MouseButton.WheelDown)
            {
                _zoom += _zoomValue;
            }
            _zoom = Mathf.Clamp(_zoom, _zoomMin, _zoomMax);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // orbit camera
        _currentYaw += _orbitVelocity * (float)delta;


        _currentPitch += _verticalVelocity * (float)delta;
        _currentPitch = Mathf.Clamp(_currentPitch, _minPitch, _maxPitch);

        RotationDegrees = new Godot.Vector3(_currentPitch, _currentYaw, RotationDegrees.Z);

        _orbitVelocity *= 0.55f;
        _verticalVelocity *= 0.55f;
        //
        // Zoom camera
        _targetZoom = Mathf.Lerp(_zoom, _targetZoom, 0.85f);
        _camera.Fov = _targetZoom;
        //
    }


}
