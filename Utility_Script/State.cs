using Godot;

public abstract class State
{
    protected Node owner;

    public State(Node owner)
    {
        this.owner = owner;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicUpdate(double delta){}
}
