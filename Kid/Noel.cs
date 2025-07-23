using Godot;
using System;
using System.Dynamic;
using System.Threading.Tasks;

public partial class Noel : CharacterBody3D
{
    private float moveSmoothing = 0.5f;
    public float movementSpeed { get; set; } = 1.5f;//to retrieve from character stat
    public float stoppingDistance { get; set; } = 2.0f;//default value
    public float ReactionSpeed { get; set; } = 1.0f;//default value
    private Vector3 _velocity;
    public Vector3 getInternalVelocity => _velocity;
    public Vector3 movementsTargetPosition { get; set; }
    public Vector3 NavTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }
    private float gravity;
    public bool move { get; set; } = true; //set Noel to move, to be manipulated by state machnine
    private bool timerCreation = false;
    private NavigationAgent3D _navigationAgent;
    private BlackBoard_Player playerBlackboard;
    private Blackboard_Noel noelBlackboard;

    public override void _Ready()
    {
        base._Ready();
        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
        noelBlackboard = GetTree().GetFirstNodeInGroup("Noel_Blackboard") as Blackboard_Noel;

        //set default point of interest
        movementsTargetPosition = playerBlackboard.GetPlayerPosition();

        //change this latter for a much more flexible approach
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        InitializeAgent();
    }
    public override void _Process(double delta)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 oldPosition = GlobalPosition;

        moveNoel(delta);

        Velocity = _velocity;
        MoveAndSlide();

        Vector3 newPosition = GlobalPosition;
        Vector3 actualMovement = newPosition - oldPosition;

        noelBlackboard.setnoelMoves(actualMovement.Length() > 0.001f);
    }
    private void moveNoel(double delta)
    {
        float buffer = 0.05f;
        float distance = GlobalPosition.DistanceTo(movementsTargetPosition);

        //debug line. remove later
        // bool shouldMove = distance > stoppingDistance + buffer;
        // GD.Print($"Distance: {distance:F2}, StoppingDistance+Buffer: {(stoppingDistance + buffer):F2}, ShouldMove: {shouldMove}");

        //is moving allowed
        if (move)
        {
            //check distance as not to get too close to target
            if (distance > stoppingDistance + buffer)
            {
                //debug line. remove later
                // GD.Print("MOVING - AgentMove called");

                if (movementsTargetPosition.LengthSquared() > 0.0001f)
                {
                    NavTarget = movementsTargetPosition;
                }
                AgentMove();
            }
            else
            {
                //debug line. remove later
                // GD.Print("STOPPING - Within stopping distance");
                _velocity.X = 0f;
                _velocity.Z = 0f;
            }
        }
        else
        {
            // GD.Print("Move not allowed - setting velocity to zero");

            _velocity.X = 0f;
            _velocity.Z = 0f;
        }

        if (!IsOnFloor())
        {
            _velocity.Y -= gravity * (float)delta;
        }
        else
        {
            _velocity.Y = 0.0f;
        }
        // GD.Print($"Final velocity magnitude: {new Vector2(_velocity.X, _velocity.Z).Length():F3}");
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
        Vector3 currentAgentPosition = GlobalPosition;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
        currentAgentPosition.Y = 0f;
        nextPathPosition.Y = 0f;

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
