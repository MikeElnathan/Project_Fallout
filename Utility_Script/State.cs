using Godot;

public abstract partial class State: Node3D
{
    public StateMachine _stateMachine;
    protected AnimationPlayer animationPlayer;
    public State() { }
    public void InjectDependencies(AnimationPlayer animationPlayer)
    {
        this.animationPlayer = animationPlayer;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicUpdate(double delta){}
}
