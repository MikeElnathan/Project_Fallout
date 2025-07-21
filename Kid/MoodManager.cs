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
    private CharacterBody3D player;
    private BlackBoard_Player playerBlackboard;
    private Vector3 playerPosition;

    public override void _Ready()
    {
        noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;

        //temporary. to be manipulated based on mood
        noel.ReactionSpeed = 0.5f;
        noel.stoppingDistance = 2.0f;
        shouldMove();
    }
    public override void _Process(double delta)
    {
        //to vary based on noel's mood
        noel.movementsTargetPosition = playerBlackboard.GetPlayerPosition();
    }
    private void shouldMove()
    {
        noel.move = true;
    }
}
