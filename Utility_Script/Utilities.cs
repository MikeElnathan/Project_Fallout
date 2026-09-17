using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;


public static class Utilities 
{
	/// <summary>
	/// Searches the specified folder for PackedScene (.tscn) files
	/// and returns their resource paths.
	/// </summary>
	/// <param name="folderPath">
	/// The Godot resource path of the folder to search.
	/// Example: "res://Trial/Trial_Level/"
	/// </param>
	/// <returns>
	/// An array containing the paths of all .tscn files found
	/// directly inside the specified folder.
	/// </returns>
	public static Godot.Collections.Array<string> GetPackedScenePaths(string folderPath)
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
	/// <summary>
	/// Creates a Button using the file name as its displayed text,
	/// optionally applies a Theme, and adds the Button to the specified parent node.
	/// </summary>
	/// <param name="parentNode">
	/// The Node that will become the parent of the newly created Button.
	/// </param>
	/// <param name="filePath">
	/// The file path used to determine the Button's displayed name.
	/// The file extension is removed from the displayed text.
	/// </param>
	/// <param name="theme">
	/// The Theme to apply to the Button. Can be null if no custom Theme is required.
	/// </param>
	/// <returns>
	/// The newly created Button.
	/// </returns>
	public static Button createSceneButton(Node parentNode, string filePath, Theme theme)
	{
		//create button, customize it with selected theme, load that scene when pressed

		string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

		Button button = new Button();
		button.Text = fileName;
		if (theme !=null)
		{
			button.Theme = theme;
		}
		parentNode.AddChild(button);

		return button;
	}
	/// <summary>
	/// Recursively searches the descendants of the specified parent node for a node
	/// with the specified name and returns it if it matches the requested type.
	/// </summary>
	/// <typeparam name="T">
	/// The type of node to search for. Must inherit from <see cref="Node"/>.
	/// </typeparam>
	/// <param name="parent">
	/// The node from which the recursive search be	gins.
	/// </param>
	/// <param name="targetName">
	/// The name of the node to search for.
	/// </param>
	/// <returns>
	/// The first descendant with the specified name that is of type <typeparamref name="T"/>.
	/// Returns <c>null</c> if no matching node is found.
	/// </returns>
	public static T  recursiveChildFinder<T>(Node parent, string targetName) where T : Node
	{
		foreach(Node child in parent.GetChildren())
		{
			if(child.Name == targetName && child is T typedChild)
			{
				return typedChild;
			}

			T result = recursiveChildFinder<T>(child, targetName);

			if(result != null) return result;
		}

		return null;
	}
/// <summary>
/// Loads and instantiates a scene from the specified path, then adds the
/// instantiated scene as a child of the specified parent node.
/// </summary>
/// <param name="parentNode">
/// The node that will become the parent of the instantiated scene.
/// </param>
/// <param name="pathName">
/// The resource path to the <c>.tscn</c> scene to load.
/// </param>
/// <param name="duplicate">
/// Determines whether an additional instance of the scene is allowed.
/// If <c>false</c>, the method searches the parent hierarchy for an existing
/// node with the same name as the scene and prevents another instance from
/// being added if one is found. If <c>true</c>, the scene is added regardless
/// of whether an instance already exists.
/// </param>
/// <remarks>
/// The scene name is derived from the file name without its extension.
/// For example, <c>res://Trial/Trial_Level/Trial_Level_1.tscn</c>
/// produces the node name <c>Trial_Level_1</c>.
/// </remarks>
	public static void LoadScene(Node parentNode, string pathName, bool duplicate)
	{
		PackedScene scene = GD.Load<PackedScene>(pathName);
		Node instance = scene.Instantiate();

		string name = System.IO.Path.GetFileNameWithoutExtension(pathName);

		if (!duplicate)//mind if there's duplicate
		{
			Node parent = recursiveChildFinder<Node>(parentNode, name);
			if(parent == null)
			{
				parentNode.AddChild(instance);
			}
			else GD.PrintErr($"Already have {name} as a child of {parentNode}");

		}
		else
		{
			parentNode.AddChild(instance);
		} 

	}
	/// <summary>
	/// Creates an asynchronous timer and waits until the specified
	/// amount of time has elapsed.
	/// </summary>
	/// <param name="node">
	/// The Node used to access the SceneTree.
	/// </param>
	/// <param name="timeInSec">
	/// The amount of time to wait, in seconds.
	/// </param>
	/// <returns>
	/// A Task that completes when the timer reaches its timeout.
	/// </returns>
	public static async Task createTimer(Node node, float timeInSec)
	{
		await node.ToSignal(node.GetTree().CreateTimer(timeInSec), SceneTreeTimer.SignalName.Timeout);
	}
	/// <summary>
	/// Loads a JSON file and converts its root data into a Godot Dictionary.
	/// </summary>
	/// <param name="jsonpath">
	/// The Godot resource path of the JSON file.
	/// Example: "res://Data/SaveData.json"
	/// </param>
	/// <returns>
	/// A Dictionary containing the JSON data if the file contains
	/// a JSON object. Returns null if the file cannot be opened
	/// or the JSON root is not a Dictionary.
	/// </returns>
	public static Dictionary loadJSONData(string jsonpath)
	{
		string path = jsonpath;

		using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);

		if(file == null)
		{
			GD.PrintErr($"Could not open : {path}");
			return null;
		}

		string jsonText = file.GetAsText();
		Variant jsonData = Json.ParseString(jsonText);

		if(jsonData.VariantType != Variant.Type.Dictionary) //this could cause problem
		{ 
			GD.PrintErr("JSON is not a dictionary");
			return null;
		}

		return jsonData.As<Dictionary>();
	}
	public static void QuitApplication(Node node)
	{
		//TODO: future, to prompt user on unsaved progress if the game is IN_GAME mode
		node.GetTree().Quit();
	}
	//TODO function that writes to error log, save in a text file
}
