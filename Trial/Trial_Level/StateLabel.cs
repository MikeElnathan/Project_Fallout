using Godot;

public partial class StateLabel : Label
{
    private BlackBoard_Player playerBlackboard;
    public override void _Ready()
    {
        playerBlackboard = BlackBoard_Player.Instance;
    }

    public override void _Process(double delta)
    {
        Text = $"Player State: {playerBlackboard.currentState}";
    }
}
