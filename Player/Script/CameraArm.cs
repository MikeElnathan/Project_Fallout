using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CameraArm : Node3D
{
    private float orbit_sensitivity = 6.5f;

    private float orbit_velocity = 0.0f;
    private float vertical_velocity = 0.0f;
    private float current_yaw = 0.0f;
    private float current_pitch = 0.0f;
    private float min_pitch = 0.0f;
    private float max_pitch = 25.0f;
    private float deadzone = 1.5f;

    private float zoom = 45.0f;
    private float zoom_value = 2.0f;
    private float zoom_min = 30.0f;
    private float zoom_max = 75.0f;
    private float target_zoom;

    private Camera3D camera;

    public override void _Ready()
    {
        current_yaw = RotationDegrees.Y;
        camera = GetChild<Camera3D>(0);
        camera.Fov = zoom;
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
                if (Math.Abs(mouseMotion.Relative.Y) > deadzone)
                {
                    vertical_velocity -= mouseMotion.Relative.Y * orbit_sensitivity;
                }
            }
        }
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.WheelUp)
            {
                zoom -= zoom_value;
            }
            if (mouseButton.ButtonIndex == MouseButton.WheelDown)
            {
                zoom += zoom_value;
            }
            zoom = Mathf.Clamp(zoom, zoom_min, zoom_max);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // orbit camera
        current_yaw += orbit_velocity * (float)delta;


        current_pitch += vertical_velocity * (float)delta;
        current_pitch = Mathf.Clamp(current_pitch, min_pitch, max_pitch);

        RotationDegrees = new Godot.Vector3(current_pitch, current_yaw, RotationDegrees.Z);

        orbit_velocity *= 0.55f;
        vertical_velocity *= 0.55f;
        //
        // Zoom camera
        target_zoom = Mathf.Lerp(zoom, target_zoom, 0.85f);
        camera.Fov = target_zoom;
        //
    }


}
