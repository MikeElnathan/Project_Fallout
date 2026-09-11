using Godot;
using System;

public partial class Walk : State
{
    public override void Enter()
    {
        animationPlayer.Play("Walk", customBlend: 0.5f);
    }
    public override void Exit()
    {
        //TODO
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
