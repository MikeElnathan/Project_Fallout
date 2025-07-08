using Godot;
using System;
using System.Threading.Tasks;

public partial class Noel : CharacterBody3D
{
    private float moveSmoothing = 0.5f;
    private float movementSpeed = 2.0f;
    private float stoppingDistance = 2.0f;
    public float ReactionSpeed { get; set; }
    private Vector3 _velocity;
    public Vector3 movementsTargetPosition { get; set; }
    public Vector3 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }
    private float gravity;
    public bool move { get; set; }
    private NavigationAgent3D _navigationAgent;
    private CharacterBody3D player;

    public override void _Ready()
    {
        base._Ready();
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D; //change this to access player character from a blackboard later on
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();//change this latter for a much more flexible approach
        InitializeAgent();
    }
    public override void _Process(double delta)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 playerPosition = player.GlobalPosition;
        float distance = GlobalPosition.DistanceTo(playerPosition);

        if (move && distance > stoppingDistance)
        {
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
        _navigationAgent.TargetDesiredDistance = stoppingDistance;
        Callable.From(ActorSetup).CallDeferred();
    }
    private void AgentMove()
    {
        Vector3 direction = Vector3.Zero;

        if (_navigationAgent.IsNavigationFinished())
        {
            return;
        }
        Vector3 currentAgentPosition = GlobalPosition;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector3 playerPosition = player.GlobalPosition;
        GD.Print("nextPathPosition",nextPathPosition);

        direction = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;

        _velocity.X = Mathf.Lerp(_velocity.X, direction.X, moveSmoothing);
        _velocity.Z = Mathf.Lerp(_velocity.Z, direction.Z, moveSmoothing);
    }
    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }
    public async Task DelayReaction(float seconds)
    {
        Timer timer = new Timer();
        timer.WaitTime = seconds;
        timer.OneShot = true;
        AddChild(timer);
        timer.Start();

        await ToSignal(timer, Timer.SignalName.Timeout);
        timer.QueueFree();
    }
}
