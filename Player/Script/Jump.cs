using Godot;
using System;

public partial class Jump : State
{

    public override void Enter()
    {
        GD.Print("Jump State entered");
        animationPlayer.Play("Jumping", customBlend: 0.2f);
    }
    public override void Exit()
    {
        GD.Print("Jump State exited");
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
