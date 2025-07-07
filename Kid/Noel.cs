using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    private NavigationAgent3D _navigationAgent;
    private float movementSpeed = 5.0f;
    private Vector3 movementsTargetPosition;
    public bool followPlayer { get; set; }
    private CharacterBody3D player;
    public Vector3 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }

    public override void _Ready()
    {
        base._Ready();
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D; //change this to access player character from a blackboard later on
        InitializeAgent();
    }
    public override void _Process(double delta)
    {
        //TODO
    }
    public override void _PhysicsProcess(double delta)
    {
        if (followPlayer)
        {
            movementsTargetPosition = player.GlobalPosition;
            MovementTarget = movementsTargetPosition;
            AgentMove();
        }
        else
        {
            Velocity = Vector3.Zero;
        }
        MoveAndSlide();
    }
    private void InitializeAgent()
    {
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _navigationAgent.PathDesiredDistance = 0.5f;
        _navigationAgent.TargetDesiredDistance = 0.5f;
        Callable.From(ActorSetup).CallDeferred();
    }
    private void AgentMove()
    {
        if (_navigationAgent.IsNavigationFinished())
        {
            return;
        }
        Vector3 currentAgentPosition = GlobalTransform.Origin;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
    }
    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }
}
