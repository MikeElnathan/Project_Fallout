using Godot;

public partial class StateMachineNoel : BaseStateMachine
{
    private BlackBoard_Player playerBlackboard;
    private CharacterBody3D noel;
    private Noel classNoel;
    private Vector3 playerPosition;
    private SignalBus signalBus;
    private SignalBus_Noel signalBus_Noel;
    public bool shouldFollow { get; set; }
    public bool noelIdle { get; set; }
    public bool sleep { get; set; }
    public bool sneak { get; set; }

    public override void _Ready()
    {
        signalBus = SignalBus.Instance;
        base._Ready();
        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player; 
        noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
        classNoel = GetTree().GetFirstNodeInGroup("Noel") as Noel; //to access method inside Noel
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        triggerStateChange();
    }
    private void triggerStateChange()
    {
        if (shouldFollow && !noelIdle)
        {
            changeState("FollowPlayer");
        }
        else if (noelIdle)
        {
            changeState("idleNoel");
        }
        GD.Print("idleNoel: ", noelIdle, ", shouldFollow: ", shouldFollow);
    }
    private float calculateDistance()
    {
        playerPosition = playerBlackboard.GetPlayerPosition();
        //change this later to get player info from a blackboard
        float distance = playerPosition.DistanceTo(noel.GlobalPosition);

        return distance;
    }
    protected override void ReadSignal()
    {
        //to improve. several condition should trigger a state change
        //this is signal received from player. To be combined with other conditions inside different method before triggering a state change.
        //memory leak from not unsubscribing to signals?
        signalBus.Walk += () => { shouldFollow = true; noelIdle = false; };
        signalBus.Idle += () => { noelIdle = true; shouldFollow = false; };
        signalBus.Sleep += () => sleep = true;
        signalBus.Sneak += () => sneak = true;
    }
    private bool shouldFollowConditions()
    {
        //add some other conditions here
        return shouldFollow;
    }
    private void resetBool()
    {
        GD.Print("Resetting flag");
        shouldFollow = false;
        sleep = false;
        sneak = false;
    }
}
