using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    public override void _Process(double delta)
    {
        base._Process(delta);
    }

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}
}
