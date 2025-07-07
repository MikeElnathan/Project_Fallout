using Godot;
using System.Threading.Tasks;

public partial class BlackBoard_Player : Node
{
    private CharacterBody3D Player;
    public Vector3 playerPosition;
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
    public override void _Process(double delta)
    {
        if (Player != null)
        {
            playerPosition = Player.GlobalPosition;
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }
}
