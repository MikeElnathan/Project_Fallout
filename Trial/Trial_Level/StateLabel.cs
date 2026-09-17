using Godot;

public partial class StateLabel : Label
{
    private BlackBoard_Player playerBlackboard;
    public override void _Ready()
    {
        playerBlackboard = GetTree().GetFirstNodeInGroup("Player_Blackboard") as BlackBoard_Player;
    }

    public override void _Process(double delta)
    {
        Text = $"Player State: {playerBlackboard.currentState}";
    }
}
