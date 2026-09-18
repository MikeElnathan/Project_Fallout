using Godot;
using System;

public partial class SceneManager : Node
{
	private Node _nodeMainMenu;
	private VBoxContainer _mainMenu;
	private VBoxContainer _loadGameMenu;
	private CanvasLayer _mainMenuCanvas;
	private Node _nodeLoadGameMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print($"SceneManager: {GetPath()}");
		init();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void init()
	{
		_nodeMainMenu = GetChild(0);
		_mainMenu = Utilities.recursiveChildFinder<VBoxContainer>(_nodeMainMenu, "Main_Menu");
		_mainMenuCanvas = GetChild(0).GetChild(0) as CanvasLayer;

		_loadGameMenu = Utilities.recursiveChildFinder<VBoxContainer>(_nodeMainMenu, "Load_Game_Menu");
		_nodeLoadGameMenu = Utilities.recursiveChildFinder<Node>(_nodeMainMenu, "Load_Game_Menu");
	}
	public void displayTrialLevels()
	{
		getTrialLevelturnToButtons();
		_mainMenu.Visible = false;
		_loadGameMenu.Visible = true;
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
		Utilities.LoadScene(this, pathName, true);
		_mainMenuCanvas.Visible = false;
	}
}
