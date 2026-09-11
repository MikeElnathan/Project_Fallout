using Godot;
using System;

public partial class RunNoel : State
{
	// Called when the node enters the scene tree for the first time.
	private Noel noel;
	private StateLabel stateText;

	public override void _Ready()
	{
		base._Ready();
		noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
		stateText = GetTree().GetFirstNodeInGroup("StateLabel") as StateLabel;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
