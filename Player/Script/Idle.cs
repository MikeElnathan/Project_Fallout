using Godot;

public partial class Idle : State
{
    public override void _Ready()
    {
        
    }

    public override void Enter()
    {
        animationPlayer.Play("Idle", customBlend: 0.2f);
    }
    public override void Exit()
    {
        //TODO
    }
    public override void Update(double delta) { }
    public override void PhysicUpdate(double delta){}
}
