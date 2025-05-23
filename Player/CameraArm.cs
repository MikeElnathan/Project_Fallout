using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CameraArm : Node3D
{
    private float orbit_sensitivity = 6.5f;

    private float orbit_velocity = 0.0f;
    private float current_yaw = 0.0f;
    private float deadzone = 0.5f;

    private float zoom = 0.0f;
    private float zoom_min = 30.0f;
    private float zoom_max = 75.0f; 

    public override void _Ready()
    {
        current_yaw = RotationDegrees.Y;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (Input.IsActionPressed("Orbit_Camera"))
            {
                if (Math.Abs(mouseMotion.Relative.X) > deadzone)
                {
                    orbit_velocity -= mouseMotion.Relative.X * orbit_sensitivity;
                }
            }
        }
        if (@event is InputEventMouseButton)
        {
            if (Input.IsMouseButtonPressed(MouseButton.WheelUp))
            {

            }
            if (Input.IsMouseButtonPressed(MouseButton.WheelDown))
            {

            }
        }


    }

    public override void _PhysicsProcess(double delta)
    {
        // orbit camera
        current_yaw = orbit_velocity * (float)delta;

        RotationDegrees = new Godot.Vector3(RotationDegrees.X, current_yaw, RotationDegrees.Z);
        //
    }


}
