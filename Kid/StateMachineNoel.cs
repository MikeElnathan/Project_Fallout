using Godot;
using System;
using System.Threading.Tasks;

public partial class StateMachineNoel : BaseStateMachine
{
    private CharacterBody3D player;
    private CharacterBody3D noel;
    private Noel classNoel;
    private Vector3 playerPosition;
    private float distanceToPlayer;
    private float distanceTreshold = 4.5f;
    private SignalBus signalBus;
    private SignalBus_Noel signalBus_Noel;
    public bool shouldFollow { get; set; }
    public bool sleep { get; set; }
    public bool sneak { get; set; }

    public override void _Ready()
    {
        signalBus = SignalBus.Instance;
        base._Ready();
        player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
        noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
        classNoel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        distanceToPlayer = calculateDistance();
        triggerStateChange();
    }
    private float calculateDistance()
    {
        //change this later to get player info from a blackboard
        float distance = player.GlobalPosition.DistanceTo(noel.GlobalPosition);

        return distance;
    }
    private async void triggerStateChange()
    {
        if (shouldFollowConditions())
        {
            changeState("FollowPlayer");
            resetBool();
            
        }
        else
        {
            changeState("idleNoel");
            resetBool();
        }
    }
    protected override void ReadSignal()
    {
        //to improve. several condition should trigger a state change
        signalBus.Walk += () => shouldFollow = true;
        signalBus.Sleep += () => sleep = true;
        signalBus.Sneak += () => sneak = true;
    }

    private bool shouldFollowConditions()
    {
        return distanceToPlayer > distanceTreshold || shouldFollow;
    }

    private void resetBool()
    {
        shouldFollow = false;
        sleep = false;
        sneak = false;
    }
}
