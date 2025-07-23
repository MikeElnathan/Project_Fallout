using Godot;
using System;
using System.Threading.Tasks;

public partial class Walk_Noel : State
{
    private Noel noel;
    private CharacterBody3D player;
    private float disTreshold = 4.0f;
    public override void _Ready()
    {
        base._Ready();
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
    }
    public override void Enter()
    {
        GD.Print("moving");
    }
    public override void Exit()
    {
        base.Exit();
    }
}
