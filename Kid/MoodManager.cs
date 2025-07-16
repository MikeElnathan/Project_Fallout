using Godot;

public partial class MoodManager : Node
{
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
    }
    public override void _Process(double delta)
    {
        //to varied based on noel's mood
        noel.movementsTargetPosition = playerBlackboard.GetPlayerPosition();
    }
}
