using Godot;
using System;

public partial class Noel : CharacterBody3D
{
	[Export]
	private NavigationAgent3D _navigationAgent;
	private NpcAi _aiNPC; //use it
	private float _walkSpeed;
	private float _runSpeed;
	private float _reactionTime;
	private Vector3 _thingsToFollow;
	private float _gravity;
	private Vector3 _velocity;

    public override void _Ready()
    {
		_gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
		readFromBlackboard();

		initNavAgent();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
	public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor())
		{
			_velocity.Y -= _gravity * (float)delta;
		}else _velocity.Y = 0f;

		moveNoel();

		Velocity = _velocity;
		MoveAndSlide();

	}
	public async void setThingsToFollow(Vector3 position)
	{
		//Call from AI
		if(_thingsToFollow != position)
		{
			// So that the nav system doesn't go crazy correcting position every frame, and the npc following more naturally.
			// Need to adjust wait time though
			await Utilities.createTimer(this, _reactionTime); 

			_thingsToFollow = position;
			_navigationAgent.TargetPosition = _thingsToFollow;
			return;
		}
		return;
	}
	private async void ActorSetup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		_navigationAgent.TargetPosition = _thingsToFollow;
	}
	private void initNavAgent()
	{
		if(_navigationAgent == null)
		{
			GD.PrintErr("No Navigation Agent 3D node found.");
			return;
		}

		_navigationAgent.PathDesiredDistance = 0.5f;
		_navigationAgent.TargetDesiredDistance = 0.5f;

		Callable.From(ActorSetup).CallDeferred();
	}
	private void moveNoel()
	{
		if (_navigationAgent.IsNavigationFinished())
    	{
       		_velocity.X = 0;
        	_velocity.Z = 0;
        	return;
    	}

		Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
		Vector3 direction = GlobalPosition.DirectionTo(nextPathPosition);

		var path = _navigationAgent.GetCurrentNavigationPath();
		
		_velocity.X = direction.X * _walkSpeed;
		_velocity.Z = direction.Z * _walkSpeed;
		
	}

	private void readFromBlackboard()
	{
		_walkSpeed = BlackBoard_Follower.WalkSpeed;
		_runSpeed = BlackBoard_Follower.RunSpeed;
		_reactionTime = BlackBoard_Follower.reactionTime;
	}
}
