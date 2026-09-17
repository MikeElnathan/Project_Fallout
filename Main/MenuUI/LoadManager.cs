using Godot;
using System;

public partial class LoadManager : Node
{
	private VBoxContainer _mainMenu;
	private VBoxContainer _loadGameMenu;
	private CanvasLayer _mainMenuCanvas;
	private Node _nodeLoadGameMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void init()
	{
		_loadGameMenu = Utilities.recursiveChildFinder<VBoxContainer>(this, "Load_Game_Menu");
		_nodeLoadGameMenu = Utilities.recursiveChildFinder<Node>(this, "Load_Game_Menu");
		_mainMenu = Utilities.recursiveChildFinder<VBoxContainer>(this, "Main_Menu");
		_mainMenuCanvas = Utilities.recursiveChildFinder<CanvasLayer>(this, "Main_Menu_Canvas");
	}
	private void getTrialLevelturnToButtons()
	{
		//get trial levels, turns them to buttons, pressing them causes to load that level
		Theme theme = ResourceLoader.Load<Theme>("res://Data/Theme/MainMenu_theme.tres");
		GD.Print($"Theme: {theme}");


		Godot.Collections.Array<string> prototypes = Utilities.GetPackedScenePaths("res://Trial/Trial_Level/");
		if (prototypes.Count == 0)
		{
			GD.PrintErr("No files of type .tscn found in the specific folder");
			return;
		}

		foreach(string prototype in prototypes)
		{
			Button button = Utilities.createSceneButton(_nodeLoadGameMenu, prototype, theme);
			button.Connect(Button.SignalName.Pressed, Callable.From(()=>loadingScene(prototype)));
		}
	}
	private void loadingScene(string pathName)
	{
		if(_mainMenuCanvas.Visible == true){_mainMenuCanvas.Visible = false;}
		Utilities.LoadScene(this, pathName, true);
		GD.Print("Loading tigerred");
	}
}
