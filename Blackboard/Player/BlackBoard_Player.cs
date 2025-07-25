using Godot;
using System.Threading.Tasks;

public partial class BlackBoard_Player : Node
{
    private CharacterBody3D Player;
    public Vector3 playerPosition { get; private set; }
    private SignalBus playerSignaBus;
    public GlobalEnum.State currentState { get; private set; }
    private static BlackBoard_Player _instance;
    public static BlackBoard_Player Instance => _instance;
    [Signal] public delegate void PlayerStateChangedEventHandler();
    public override void _Ready()
    {
        if (_instance != null && _instance != this)
        {
            QueueFree();
            return;
        }
        _instance = this;
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
    public void SetStateInPlayerBlackboard(GlobalEnum.State state)
    {
        if (currentState != state)
        {
            currentState = state;
            EmitSignal(SignalName.PlayerStateChanged);
        }
        else return;
    }

}
