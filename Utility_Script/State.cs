using System;
using Godot;

public abstract partial class State: Node3D
{
    public BaseStateMachine _stateMachine;
    protected AnimationPlayer animationPlayer;
    protected CharacterBody3D Actor;
    public State() { }
    public override void _Ready()
    {
        base._Ready();
    }

    public void InjectDependencies(AnimationPlayer animationPlayer)
    {
        this.animationPlayer = animationPlayer;
        Actor = _stateMachine._owner ?? throw new InvalidOperationException("Owner is null");
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicUpdate(double delta){}
}
