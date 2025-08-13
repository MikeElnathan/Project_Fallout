using Godot;
using System.Threading.Tasks;

public static class Utilities
{
    public static async Task DelaySeconds(Node node, float seconds)
    {
        await node.ToSignal(node.GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
    }
}
