using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;


public static class Utilities 
{
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
	public static Button createButton(Node parentNode, string filePath, Theme theme)
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
	public static Node recursiveChildFinder(Node parent, string targetName)
	{
		foreach(Node child in parent.GetChildren())
		{
			if(child.Name == targetName)
			{
				return child;
			}

			Node result = recursiveChildFinder(child, targetName);

			if(result != null)
			{
				GD.Print("we can't find your child.");
				return result;
			}
		}

		return null;
	}
	public static void LoadScene(String pathName, ref PackedScene sceneName)
	{
		sceneName = GD.Load<PackedScene>(pathName);
	}
	public static async Task createTimer(Node node, float timeInSec)
	{
		await node.ToSignal(node.GetTree().CreateTimer(timeInSec), SceneTreeTimer.SignalName.Timeout);
	}
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
	//TODO function that writes to error log, save in a text file
}
