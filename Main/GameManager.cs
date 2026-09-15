using Godot;
using System;

public partial class GameManager : Node
{
    private PackedScene TrialLevel;
    public GameState globalEnum;

    public override void _Ready()
    {
        //do something
        LoadTrial();
    }
    private void LoadTrial()
    {
        Utilities.LoadScene("res://Trial/Trial_Level/trial_level_3.tscn", ref TrialLevel);
        if (TrialLevel != null)
        {
            Node3D trialLevel = TrialLevel.Instantiate<Node3D>();
            AddChild(trialLevel);
        }
        else
        {
            throw new Exception("empty level warning");
        }

    }
}
