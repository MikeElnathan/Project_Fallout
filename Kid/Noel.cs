using Godot;
using System;

public partial class Noel : CharacterBody3D
{
    private Vector3 _velocity;
    private float gravity;
    private NavigationAgent3D _navigationAgent;
    private float movementSpeed = 5.0f;
    private Vector3 movementsTargetPosition;
    private CharacterBody3D player;
    public bool followPlayer { get; set; }
    public Vector3 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }

    public override void _Ready()
    {
        base._Ready();
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D; //change this to access player character from a blackboard later on
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
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
            _velocity.X = 0f;
            _velocity.Z = 0f;
        }

        if (!IsOnFloor())
        {
            _velocity.Y -= gravity * (float)delta;
        }
        else
        {
            _velocity.Y = 0.01f;
        }

        Velocity = _velocity;
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
        Vector3 direction;
        if (_navigationAgent.IsNavigationFinished())
        {
            return;
        }
        Vector3 currentAgentPosition = GlobalTransform.Origin;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

        direction = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
        _velocity.X = direction.X;
        _velocity.Z = direction.Z;
    }
    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }
}
