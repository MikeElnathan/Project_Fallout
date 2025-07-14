using Godot;
using System;

public partial class Run : State
{

    public override void Enter()
    {
        GD.Print("Player: Run state");
        animationPlayer.Play("Run", customBlend: 0.3f);
    }
    public override void Exit()
    {
        //TODO
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
