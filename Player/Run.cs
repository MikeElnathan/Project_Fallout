using Godot;
using System;

public partial class Run : State
{

    public override void Enter()
    {
        GD.Print("Run State entered");
        animationPlayer.Play("Run", customBlend: 0.3f);
    }
    public override void Exit()
    {
        GD.Print("Run State exited");
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
