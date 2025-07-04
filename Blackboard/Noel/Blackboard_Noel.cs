using Godot;
using System.Threading.Tasks;

public partial class Blackboard_Noel : Node
{
    private CharacterBody3D Noel;
    public Vector3 noelPosition;

    public override void _Ready()
    {
        _ = GetNoel();
    }
    private async Task GetNoel()
    {
        while (Noel == null || Noel.IsInsideTree())
        {
            Noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
        GD.Print("Noel: ", Noel);
    }
    public override void _Process(double delta)
    {
        if (Noel != null)
        {
            noelPosition = Noel.GlobalPosition;
        }
    }

    public Vector3 GetNoelPosition()
    {
        return noelPosition;
    }
}
