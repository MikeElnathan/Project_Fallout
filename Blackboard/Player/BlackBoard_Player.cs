using Godot;
using System.Threading.Tasks;

public partial class BlackBoard_Player : Node
{
    private CharacterBody3D Player;
    public Vector3 playerPosition;
    private SignalBus playerSignaBus;
    public SignalBus.ActionType currentState { get; private set; }
    [Signal]public delegate void PlayerStateChangedEventHandler();
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
        if (Player != null)
        {
            playerPosition = Player.GlobalPosition;
        }
        else
        {
            playerPosition = Vector3.Zero;
        }

        return playerPosition;
    }
    public void SetStateInPlayerBlackboard(SignalBus.ActionType state)
    {
        if (currentState != state)
        {
            currentState = state;
            EmitSignal(SignalName.PlayerStateChanged);
        }
        else return;
    }

}
