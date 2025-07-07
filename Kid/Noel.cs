using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    private NavigationAgent3D _navigationAgent;
    private float movementSpeed = 5.0f;
    private Vector3 movementsTargetPosition;
    public Vector3 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }

    public override void _Ready()
    {
        base._Ready();
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _navigationAgent.PathDesiredDistance = 0.5f;
        _navigationAgent.TargetDesiredDistance = 0.5f;

        movementsTargetPosition = new Vector3(0, 0, 0);

        Callable.From(ActorSetup).CallDeferred();
    }
    public override void _Process(double delta)
    {
        //TODO
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (_navigationAgent.IsNavigationFinished())
        {
            return;
        }
        Vector3 currentAgentPosition = GlobalTransform.Origin;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
        MoveAndSlide();
    }
    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        MovementTarget = movementsTargetPosition;
    }
}
