using Godot;

public partial class StateMachineNoel : BaseStateMachine
{
    private BlackBoard_Player playerBlackboard;
    private CharacterBody3D noel;
    private Noel classNoel;
    private Vector3 playerPosition;
    private SignalBus_Noel signalBus_Noel;
    private SignalBus.ActionType PlayerState;

    public override void _Ready()
    {
        playerBlackboard = BlackBoard_Player.Instance;
        signalBus_Noel = SignalBus_Noel.Instance_noel;
        noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
        classNoel = GetTree().GetFirstNodeInGroup("Noel") as Noel; //to access method inside Noel
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
        //set player state
        if (PlayerState != signalBus_Noel.curentPlayerState)
        {
            PlayerState = signalBus_Noel.curentPlayerState;
        }

        switch (PlayerState)
        {
            case SignalBus.ActionType.Idle:
                changeState("idleNoel");
                break;
            case SignalBus.ActionType.Walk:
                changeState("walkNoel");
                break;
            case SignalBus.ActionType.Sleep:
                changeState("sleepNoel");
                break;
            case SignalBus.ActionType.Run:
                //test only
                changeState("idleNoel");
                GD.Print("Run is triggered in SM");
                break;
        } 
    }
}
