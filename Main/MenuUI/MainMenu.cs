using Godot;
using System;
using System.Net.Sockets;

public partial class MainMenu : Control
{
	private SceneManager _sceneManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sceneManager = GetParent() as SceneManager;
		GD.Print($"_sceneManager: {_sceneManager}");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	//Listen to button pressed-----------------------------------------------------
	private void _on_new_game_pressed()
	{
		GD.Print("new game button pressed");
	}
	private void _on_load_game_pressed()
	{
		//For now, return all prototype level
		GD.Print("load game pressed");
		_sceneManager.displayTrialLevels();
	}
	private void _on_settings_pressed()
	{
		GD.Print("settings button pressed");

	}
	private void _on_quit_pressed()
	{
		GD.Print("Quit is pressed");
		Utilities.QuitApplication(this);
	}
	
}
