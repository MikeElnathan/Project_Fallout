using Godot;
using System.Threading.Tasks;

public partial class Noel : CharacterBody3D
{
    //---Configurable Parameters---
    private float _moveSmoothing = 0.5f;
    public float movementSpeed { get; set; } = 1.5f;//to retrieve from character stat
    public float stoppingDistance { get; set; } = 2.0f;//default value
    public float ReactionSpeed { get; set; } = 1.0f;//default value

    //--Runtime State---
    private Vector3 _velocity;
    public Vector3 movementsTargetPosition { get; set; }
    private Vector3 _navTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }
    private float _gravity;
    private bool _move = true;
    private bool _timerCreation = false;

    //---External Reference---
    private NavigationAgent3D _navigationAgent;
    private BlackBoard_Player _playerBlackboard;
    private GlobalEnum.State _currentPlayerState;
    private Blackboard_Noel _noelBlackboard;
    private StateMachineNoel _noelSM;
    private SignalBus_Noel noelSignalBus;//unused

    public override void _Ready()
    {
        base._Ready();
        _playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
        _noelBlackboard = GetTree().GetFirstNodeInGroup("Noel_Blackboard") as Blackboard_Noel;
        _noelSM = GetTree().GetFirstNodeInGroup("noelSM") as StateMachineNoel;
        noelSignalBus = SignalBus_Noel.Instance_noel;

        //change this latter for a much more flexible approach
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _initializeAgent();

        //test
        _noelSM.SetFocus(GlobalEnum.Focus.Player);
    }
    public override void _PhysicsProcess(double delta)
    {
        _checkPlayerState();
        _moveNoel(delta);

        Velocity = _velocity;
        MoveAndSlide();
    }

    //Consider changing this when there is dedicated Ai managing Noel focus
    private void _checkPlayerState() => _move = _playerBlackboard.currentState != GlobalEnum.State.Jump;

    private void _moveNoel(double delta)
    {
        float buffer = 0.05f;
        float distance = GlobalPosition.DistanceTo(movementsTargetPosition);
        if (_move)
        {
            if (distance > stoppingDistance + buffer)
            {
                if (movementsTargetPosition.LengthSquared() > 0.0001f)
                {
                    _navTarget = movementsTargetPosition;
                }
                _agentMove();
            }
            else
            {
                _stopHorizontalMovement();
            }
        }
        else
        {
            _stopHorizontalMovement();
        }

        if (!IsOnFloor())
        {
            _velocity.Y -= _gravity * (float)delta;
        }
        else
        {
            _velocity.Y = 0.0f;
        }
    }
    private void _stopHorizontalMovement()
    {
        _velocity.X = 0f;
        _velocity.Z = 0f;
    }
    private void _initializeAgent()
    {
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _navigationAgent.PathDesiredDistance = 0.5f;
        _navigationAgent.TargetDesiredDistance = stoppingDistance;
        Callable.From(_actorSetup).CallDeferred();
    }
    private async void _actorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }
    private void _agentMove()
    {
        Vector3 direction = Vector3.Zero;
        Vector3 currentAgentPosition = GlobalPosition;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

        direction = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;

        _velocity.X = Mathf.Lerp(_velocity.X, direction.X, _moveSmoothing);
        _velocity.Z = Mathf.Lerp(_velocity.Z, direction.Z, _moveSmoothing);
    }
    
    //not used.Keep it here.
    public async Task DelayReaction(float seconds)
    {
        if (_timerCreation)
        {
            return;
        }

        _timerCreation = true;

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
        _timerCreation = false;
    }
}
