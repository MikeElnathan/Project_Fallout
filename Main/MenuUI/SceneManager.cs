using Godot;
using System;

public partial class SceneManager : Node
{
	private Node _nodeMainMenu;
	public GameState gameState {get; private set;}
	[Signal]public delegate void gameStateChangedEventHandler();

	private VBoxContainer _mainMenu;
	private VBoxContainer _loadGameMenu;
	private CanvasLayer _mainMenuCanvas;
	private Node _nodeLoadGameMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("SceneManager");
		init();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("ui_cancel"))
		{
			switch (gameState)
			{
				case GameState.IN_GAME:
					gameStateSet(GameState.MAIN_MENU);
					break;
				case GameState.SHOW_SAVES:
					gameStateSet(GameState.MAIN_MENU);
					break;
			}
		}
    }
	private void init()
	{
		_nodeMainMenu = GetChild(0);
		_nodeLoadGameMenu = Utilities.recursiveChildFinder<Node>(_nodeMainMenu, "Load_Game_Menu");

		_mainMenuCanvas = GetChild(0).GetChild(0) as CanvasLayer;
		_mainMenu = Utilities.recursiveChildFinder<VBoxContainer>(_nodeMainMenu, "Main_Menu");
		_loadGameMenu = Utilities.recursiveChildFinder<VBoxContainer>(_nodeMainMenu, "Load_Game_Menu");
	}
	private void gameStateSet(GameState state)
	{
		switch (state){
			case GameState.MAIN_MENU:
				gameState = state;
				EmitSignal(SignalName.gameStateChanged);
				_mainMenuCanvas.Visible = true;
				_mainMenu.Visible = true;
				_loadGameMenu.Visible = !_mainMenu.Visible;
				break;
			case GameState.SHOW_SAVES:
				gameState = state;
				EmitSignal(SignalName.gameStateChanged);
				_loadGameMenu.Visible = true;
				_mainMenu.Visible = !_loadGameMenu.Visible;
				break;
			case GameState.NEW_GAME:
				gameState = state;
				EmitSignal(SignalName.gameStateChanged);
				break;
			case GameState.IN_GAME:
				gameState = state;
				EmitSignal(SignalName.gameStateChanged);
				_mainMenuCanvas.Visible = false;
				break;
		}
	}
	//Prototype
	public void displayTrialLevels()
	{
		getTrialLevelturnToButtons();
		gameStateSet(GameState.SHOW_SAVES);

	}
	private void loadingScene(string pathName)
	{
		Utilities.LoadScene(this, pathName, true);
		gameStateSet(GameState.IN_GAME);
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
			if(button == null){continue;}

			button.Connect(Button.SignalName.Pressed, Callable.From(()=>loadingScene(prototype)));
		}
	}
}
