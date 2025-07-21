using Godot;

public partial class StateLabel : Label
{
    private BlackBoard_Player playerBlackboard;
    private Blackboard_Noel noelBlackboard;
    public override void _Ready()
    {
        playerBlackboard = BlackBoard_Player.Instance;
        noelBlackboard = Blackboard_Noel.Instance_noel;
    }

    public override void _Process(double delta)
    {
        Text = $"Player State: {playerBlackboard.currentState}\nNoel State:{noelBlackboard.noelCurrentState}";
    }
}
