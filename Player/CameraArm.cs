using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CameraArm : Node3D
{
    private float orbit_speed;
    private float orbit_angle;


    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (Input.IsActionPressed("Orbit_Camera"))
            {
                orbit_angle += mouseMotion.Relative.X;
            }

            if (orbit_angle > 0 || orbit_angle < 0)
            {
                RotationDegrees = new Vector3(GlobalRotationDegrees.X, orbit_angle, RotationDegrees.Z);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }


}
