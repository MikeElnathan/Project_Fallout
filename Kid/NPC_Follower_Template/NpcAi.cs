using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;


public partial class NpcAi : Node
{
	[Export]
	public Resource npcBlackboard {get; set;}
	private Dictionary CharacterStat;
	[Export(PropertyHint.File, "*.json")]
	private string _saveDataStringPath;


	public override void _Ready()
	{
		getSaveFiles();
	}
	public override void _Process(double delta)
	{
		
	}

	//Get save file then write it to blackboard. Should check for any  save files, otherwise, will resort to default stat and position, ala new game mode
	private void getSaveFiles()
	{
		if (_saveDataStringPath == null){
			GD.PrintErr("No Save Path found / Save Path is Null");
			return;
		}

		Dictionary data =  Utilities.loadJSONData(_saveDataStringPath);

		//lets try this first
		Dictionary movement = data["movement"].As<Dictionary>();

		BlackBoard_Follower.WalkSpeed = (float)movement["walk_speed"];
		BlackBoard_Follower.RunSpeed = (float)movement["run_speed"];

	}
}
