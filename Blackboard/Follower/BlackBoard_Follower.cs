using Godot;
using Godot.Collections;
using System;


[GlobalClass]
public partial class BlackBoard_Follower : Resource //Should have named it Blackboard_NPC for general use. oh well.
{
	[Export]
	public NPCState npcState {get; private set;}
	[Signal]public delegate void StateChangedEventHandler();

	[Export]
	public Vector3 npcPosition {get; private set;}
	[Export]
	public NPCFollowTarget npcFollowTarget {get; private set;}
	public static float WalkSpeed;
	public static float RunSpeed;
	public static float reactionTime;
	
	
	public void SetNPCState(NPCState state)
	{
		if(npcState != state)
		{
			npcState = state;
			State();
		}
		else return;
	}
	public void State()
	{
		EmitSignal(SignalName.StateChanged);
	}
	public void setNPCPosition(Vector3 position)
	{
		npcPosition = position;
	}
	public void setNPCFollowTarget(NPCFollowTarget followTarget)
	{
		if(npcFollowTarget != followTarget)
		{
			npcFollowTarget = followTarget;
		}
	}
}
