using Godot;
using System.Threading.Tasks;

public partial class BlackBoard_Player : Node
{
    public enum PlayerState
    {
        Idle, Walk, Jump, Run, Sneak, Sleep
    }
    private CharacterBody3D Player;
    public Vector3 playerPosition;
    public PlayerState currentState { get; private set; }
    public override void _Ready()
    {
        _ = GetPlayer();
    }
    private async Task GetPlayer()
    {
        while (Player == null || !Player.IsInsideTree())
        {
            Player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D;
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return Player.GlobalPosition;
    }
    public void SetStateInPlayerBlackboard(PlayerState state)
    {
        if (currentState != state)
        {
            currentState = state;
        }
        else return;
    }
}
