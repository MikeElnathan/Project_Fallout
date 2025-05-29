using Godot;
using System;

public partial class Idle : State
{
    public override void Enter()
    {
        GD.Print("Idle State entered");
    }
    public override void Exit()
    {
        GD.Print("Idle State exited");
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
