using Godot;
using System;

public partial class MainMenu : Control
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
	}
	private void _on_settings_pressed()
	{
		GD.Print("settings button pressed");
	}

	//Helper functions------------------------------------------------------------
	private void createButtonBasedOnSceneToBeLoaded(string filePath, Resource theme)
	{
		//create button, customize it with selected theme, load that scene when pressed

	}
	private Godot.Collections.Array<string> GetPackedScenePaths(string folderPath)
	{
		var scenePaths = new Godot.Collections.Array<string>();

		DirAccess dir = DirAccess.Open(folderPath);

		if(dir == null)
		{
			GD.PrintErr($"Could not open {folderPath}");
			return scenePaths;
		}

		dir.ListDirBegin();

		string fileName = dir.GetNext();

		while(fileName != "")
		{
			if(!dir.CurrentIsDir() && fileName.EndsWith(".tscn"))
			{
				scenePaths.Add($"{folderPath}/{fileName}");
			}
			fileName = dir.GetNext();
		}

		dir.ListDirEnd();

		return scenePaths;
	}

	//For prototyping----------------------------
	private void getTrialLevelturnToButtons()
	{
		//get trial levels, turns them to buttons, pressing them causes to load that level

	}
}
