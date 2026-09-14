using Godot;
using Godot.Collections;
using System;


[GlobalClass]
public partial class BlackBoard_Follower : Resource //Should have named it Blackboard_NPC for general use. oh well.
{
	[Export]
	public NPCState npcState {get; private set;}
	[Export]
	public Vector3 npcPosition {get; private set;}
	[Export]
	public NPCFollowTarget npcFollowTarget {get; private set;}
	public static float WalkSpeed;
	public static float RunSpeed;
	
	
	public void SetNPCState(NPCState state)
	{
		if(npcState != state)
		{
			npcState = state;
		}
		else return;
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
