using Godot;

public partial class StateMachineNoel : BaseStateMachine
{
    private BlackBoard_Player playerBlackboard;
    private Blackboard_Noel noelBlackboard;
    private CharacterBody3D noel;
    private Noel classNoel;
    private Vector3 noelVelocity = Vector3.Zero;
    private Vector3 playerPosition;
    private SignalBus_Noel signalBus_Noel;
    private SignalBus.ActionType PlayerState;

    public override void _Ready()
    {
        playerBlackboard = BlackBoard_Player.Instance;
        signalBus_Noel = SignalBus_Noel.Instance_noel;
        noelBlackboard = Blackboard_Noel.Instance_noel;
        base._Ready();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
    protected override void ReadSignal()
    {
        base.ReadSignal();
        signalBus_Noel.Connect(SignalBus_Noel.SignalName.PlayerStateSignal, new Callable(this, nameof(StateChangeParameter)));
    }
    private void StateChangeParameter()
    {
        noelVelocity = noelBlackboard.GetNoelVelocity();
        //set player state
        if (PlayerState != signalBus_Noel.curentPlayerState)
        {
            PlayerState = signalBus_Noel.curentPlayerState;
        }
        //switching to Idle state base on horizintal velocity
        float horizontalSpeed = new Vector2(noelVelocity.X, noelVelocity.Z).Length();

        if (horizontalSpeed <= 0.1f)
        {
            changeState("walkNoel");
            noelBlackboard.noelCurrentState = SignalBus.ActionType.Walk;

        }
        else if (horizontalSpeed > 0.1f)
        {
            changeState("idleNoel");
            noelBlackboard.noelCurrentState = SignalBus.ActionType.Idle;
        }
    }
}
