using Godot;
using System;

public partial class Walk : State
{
    

    public override void Enter()
    {
        GD.Print("Walk State entered");
        animationPlayer.Play("Walk", customBlend: 0.5f);
    }
    public override void Exit()
    {
        GD.Print("Walk State exited");
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
