using Godot;
using System;
using System.Dynamic;
using System.Threading.Tasks;

public partial class Noel : CharacterBody3D
{
    private float moveSmoothing = 0.5f;
    private float movementSpeed = 2.0f;
    public float stoppingDistance { get; set; } = 2.0f;//default value
    public float ReactionSpeed { get; set; } = 1.0f;//default value
    private Vector3 _velocity;
    public Vector3 movementsTargetPosition { get; set; }
    public Vector3 NavTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }
    private float gravity;
    public bool move { get; set; } = false; //set Noel to move, to be manipulated by state machnine
    private bool timerCreation = false;
    private NavigationAgent3D _navigationAgent;
    private BlackBoard_Player playerBlackboard;

    public override void _Ready()
    {
        base._Ready();
        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;

        //change this latter for a much more flexible approach
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        InitializeAgent();
    }
    public override void _Process(double delta)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        moveNoel(delta);

        Velocity = _velocity;
        MoveAndSlide();
    }
    private void moveNoel(double delta)
    {
        Vector3 playerPosition = playerBlackboard.GetPlayerPosition();
        float distance = GlobalPosition.DistanceTo(playerPosition);

        //how far from player is Noel to trigger followPlayer
        if (move && distance > stoppingDistance)
        {
            if (movementsTargetPosition.LengthSquared() > 0.0001f)
            {
                NavTarget = movementsTargetPosition;
            }
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
    }
    private void InitializeAgent()
    {
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _navigationAgent.PathDesiredDistance = 0.5f;
        _navigationAgent.TargetDesiredDistance = stoppingDistance;
        Callable.From(ActorSetup).CallDeferred();
    }
    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
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

        direction = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;

        _velocity.X = Mathf.Lerp(_velocity.X, direction.X, moveSmoothing);
        _velocity.Z = Mathf.Lerp(_velocity.Z, direction.Z, moveSmoothing);
    }
    public async Task DelayReaction(float seconds)
    {
        //GD.Print("Timer created");
        //_________________________

        if (timerCreation)
        {
            return;
        }
        
        timerCreation = true;

        Timer timer = new Timer
        {
            Name = "delayTimer",
            WaitTime = seconds,
            OneShot = true
        };
        AddChild(timer);
        timer.Start();

        await ToSignal(timer, Timer.SignalName.Timeout);
        timer.QueueFree();
        timerCreation = false;

        //__________________________
        //GD.Print("Timer destroyed");
    }
}
