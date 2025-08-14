using Godot;
using System;

public partial class GameManager : Node
{
    private PackedScene _trialLevel;

    public override void _Ready()
    {
        //do something
        LoadTrial();
    }
    private void LoadTrial()
    {
        LoadScene("res://Trial/Trial_Level/trial_level_1.tscn", ref _trialLevel);
        if (_trialLevel != null)
        {
            Node3D trialLevel = _trialLevel.Instantiate<Node3D>();
            AddChild(trialLevel);
        }
        else
        {
            throw new Exception("empty level warning");
        }
    }
    private void LoadScene(String pathName, ref PackedScene sceneName)
    {
        sceneName = GD.Load<PackedScene>(pathName);
    }
}
