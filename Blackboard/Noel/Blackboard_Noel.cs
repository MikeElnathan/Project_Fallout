using Godot;
using System.Threading.Tasks;

public partial class Blackboard_Noel : Node
{
    private CharacterBody3D Noel;
    public Vector3 noelPosition { get; private set; }
    private SignalBus_Noel noelSignalBus;
    //Noel mood here
    private static Blackboard_Noel _instance;
    public static Blackboard_Noel Instance_noel => _instance;
    

    public override void _Ready()
    {
        _ = GetNoel();
    }
    private async Task GetNoel()
    {
        //make sure to get the node when it's loaded
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
