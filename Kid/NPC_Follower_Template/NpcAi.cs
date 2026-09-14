using Godot;
using Godot.Collections;


public partial class NpcAi : Node
{
	[Export]
	public Resource npcBlackboard {get; set;}
	private Dictionary CharacterStat;
	[Export(PropertyHint.File, "*.json")]
	private string _saveDataStringPath;

	public static Vector3 _playerPosition; //KIV
	private BlackBoard_Player blackBoard_Player;
	private Noel _noel;


	public override void _Ready()
	{
		_noel = GetParent<Noel>() as Noel;

		getSaveFiles();
		getPlayerBlackboard();

		//_playerPosition = new Vector3(0, 0, 0); //default
	}
	public override void _Process(double delta)
	{
		_playerPosition = blackBoard_Player.GetPlayerPosition();
		_noel.setThingsToFollow(_playerPosition);
	}
	private void getPlayerBlackboard()
	{
		blackBoard_Player = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
		
		if(blackBoard_Player == null)
		{
			GD.PrintErr("NpcAi (noel's) can't find player's blackboard");
			return;
		}
	}
	private void giveSomethingToFollow()
	{
		//temporary test

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
