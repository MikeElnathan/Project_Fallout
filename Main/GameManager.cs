using Godot;
using System;

public partial class GameManager : Node3D
{
    private PackedScene TrialLevel;

    public override void _Ready()
    {
        //do something
        LoadTrial();
    }

    private void LoadTrial()
    {
        LoadScene("res://Trial/Trial_Level/trial_level_1.tscn", ref TrialLevel);
        if (TrialLevel != null)
        {
            Node3D trialLevel = TrialLevel.Instantiate<Node3D>();
            AddChild(trialLevel);
            GD.Print("it....work?");
        }
        else
        {
            GD.Print("This doesn't work");
        }

    }

    private void LoadScene(String pathName, ref PackedScene sceneName)
    {
        sceneName = GD.Load<PackedScene>(pathName);
    }
}
