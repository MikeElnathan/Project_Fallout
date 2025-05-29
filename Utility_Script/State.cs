using Godot;

public abstract partial class State: Node3D
{
    public StateMachine _stateMachine;
    public State() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicUpdate(double delta){}
}
