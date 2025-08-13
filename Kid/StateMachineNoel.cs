using Godot;

public partial class StateMachineNoel : BaseStateMachine
{
    //---Public API---
    public GlobalEnum.Focus focus { get; private set; }

    //---Internal State---
    private GlobalEnum.State _playerState;

    //---External Reference---
    private BlackBoard_Player _playerBlackboard;
    private Blackboard_Noel _noelBlackboard;
    private CharacterBody3D _noel;
    private Noel _classNoel;
    private SignalBus_Noel _signalBus_Noel;

    public override void _Ready()
    {
        initialize();
        base._Ready();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _stateChangeManager();
    }
    private void initialize()
    {
        _signalBus_Noel = SignalBus_Noel.Instance_noel;
        _noelBlackboard = Blackboard_Noel.Instance_noel;
        _playerBlackboard = BlackBoard_Player.Instance;

        _noel = GetParent() as CharacterBody3D;
        _classNoel = GetParent() as Noel;
    }
    protected override void ReadSignal()
    {
        _signalBus_Noel.Connect(SignalBus_Noel.SignalName.PlayerStateSignal, new Callable(this, nameof(setPlayerState)));
    }
    private void setPlayerState() => _playerState = _playerBlackboard.currentState;
    public void SetFocus(GlobalEnum.Focus _focus) => focus = _focus;

    private void _stateChangeManager()
    {
        _classNoel.movementsTargetPosition = _playerBlackboard.GetPlayerPosition();
        bool _isNoelIdle = _noel.Velocity.X == 0f && _noel.Velocity.Z == 0f;

        if (_isNoelIdle)
        {
            changeState("idleNoel");
        }
        else
        {
            //to change
            changeState("walkNoel");
        }
    }
}
