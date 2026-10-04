using Godot;
using System;

public partial class Level : Node
{
	private SceneManager _sceneManager;
	
	public override void _Ready()
	{
    	Node found = GetTree().GetFirstNodeInGroup("SceneManager");

    	_sceneManager = found as SceneManager;

		_sceneManager.Connect(SceneManager.SignalName.gameStateChanged, Callable.From(respondToGameState));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	protected virtual void respondToGameState()
	{
		switch (_sceneManager.gameState)
		{
			case GameState.IN_GAME_MENU:
				break;
			case GameState.MAIN_MENU:
				this.QueueFree();
				break;
		}
	}
}
