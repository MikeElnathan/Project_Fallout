using Godot;
using System;

public partial class Jump : State
{
    private float _jumpDuration;

    public override void _Ready()
    {
        base._Ready();
    }
    public override void Enter()
    {
        animationPlayer.Play("Jumping", customBlend: 0.2f);
    }
    public override void Exit()
    {
        //TODO
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
