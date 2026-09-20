using Godot;
using System;

public partial class Level : Node
{
	private SceneManager _sceneManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print($"LEVEL READY: {Name} | Instance: {GetInstanceId()} | Path: {GetPath()}");

    	// GD.Print($"SceneManager group count: {GetTree().GetNodeCountInGroup("SceneManager")}");

    	Node found = GetTree().GetFirstNodeInGroup("SceneManager");

    	// GD.Print($"Found SceneManager: {found}");

    	// if (found != null)
    	// {
        // 	GD.Print($"Found path: {found.GetPath()}");
        // 	GD.Print($"Found type: {found.GetType()}");
        // 	GD.Print($"Is SceneManager group: {found.IsInGroup("SceneManager")}");
   	 	// }

    	_sceneManager = found as SceneManager;

    	// GD.Print($"_sceneManager: {_sceneManager}");
		_sceneManager.Connect(SceneManager.SignalName.gameStateChanged, Callable.From(respondToGameState));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	protected virtual void respondToGameState()
	{
		// GD.Print(
        // 	$"signal received | " +
        // 	$"Level Instance: {GetInstanceId()} | " +
        // 	$"Path: {GetPath()} | " +
        // 	$"GameState: {_sceneManager.gameState}"
    	//);
		switch (_sceneManager.gameState)
		{
			case GameState.IN_GAME_MENU:
				break;
			case GameState.MAIN_MENU:
				// GD.Print(
                // $"Im freeing this node | " +
                // $"Instance: {GetInstanceId()}"
            	// 	);
				this.QueueFree();
				break;
		}
	}
}
