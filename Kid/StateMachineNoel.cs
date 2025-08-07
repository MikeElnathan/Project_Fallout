using Godot;

public partial class StateMachineNoel : BaseStateMachine
{
    public GlobalEnum.Focus _focus { get; private set; }
    private GlobalEnum.State playerState;
    private BlackBoard_Player playerBlackboard;
    private Blackboard_Noel noelBlackboard;
    private CharacterBody3D noel;
    private Noel classNoel;
    private SignalBus_Noel signalBus_Noel;

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
        StateChangeManager();
    }

    private void initialize()
    {
        signalBus_Noel = SignalBus_Noel.Instance_noel;
        noelBlackboard = Blackboard_Noel.Instance_noel;
        playerBlackboard = BlackBoard_Player.Instance;

        noel = GetParent() as CharacterBody3D;
        classNoel = GetParent() as Noel;
    }
    protected override void ReadSignal()
    {
        signalBus_Noel.Connect(SignalBus_Noel.SignalName.PlayerStateSignal, new Callable(this, nameof(setPlayerState)));
    }

    private void setPlayerState() => playerState = playerBlackboard.currentState;
    public void SetFocus(GlobalEnum.Focus focus) => _focus = focus;

    private void StateChangeManager()
    {
        if (_focus == GlobalEnum.Focus.Player)
        {
            classNoel.movementsTargetPosition = playerBlackboard.GetPlayerPosition();
            changeState("walkNoel");
            if (noel.Velocity.X == 0f && noel.Velocity.Y == 0f)
            {
                changeState("idleNoel");
            }
        }
        else if (_focus == GlobalEnum.Focus.Noel)
        {
            GD.Print("focus is on noel");
        }
    }
}
