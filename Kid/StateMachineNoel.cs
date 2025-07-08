using Godot;
using System;

public partial class StateMachineNoel : BaseStateMachine
{
    private CharacterBody3D player;
    private CharacterBody3D noel;
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
    private void triggerStateChange()
    {
        if (shouldFollowConditions())
        {
            changeState("FollowPlayer");
        }
        else
        {
            changeState("idleNoel");
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
}
