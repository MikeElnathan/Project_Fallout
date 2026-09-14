using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public static class Utilities
{
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
