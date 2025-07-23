using Godot;

public partial class MoodManager : Node
{
    public enum NoelMoods
    {
        Neutral,
        Happy,
        Sad,
        Angry,
        Afraid,
        Excited
    }
    private Noel noel;
    public NoelMoods noelMood { get; private set; }
    private CharacterBody3D player;
    private BlackBoard_Player playerBlackboard;

    public override void _Ready()
    {
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
    }
    public void setNoelMood(NoelMoods mood)
    {
        noelMood = mood;
    }
    public override void _Process(double delta)
    {
        noel.movementsTargetPosition = playerBlackboard.GetPlayerPosition();
        base._Process(delta);
    }

}
