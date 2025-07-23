using Godot;
using System.Threading.Tasks;

public partial class Blackboard_Noel : Node
{
    private CharacterBody3D Noel;
    private Noel noel;
    public bool noelMoving { get; private set; }
    public SignalBus.ActionType noelCurrentState { get; set; }
    public Vector3 noelPosition { get; private set; }
    public Vector3 noelVelocity { get; private set; }
    public SignalBus.ActionType NoelState { get; }
    private SignalBus_Noel noelSignalBus;
    //Noel mood here
    private static Blackboard_Noel _instance;
    public static Blackboard_Noel Instance_noel => _instance;

    public override void _Ready()
    {
        if (_instance != null && _instance != this)
        {
            QueueFree();
            return;
        }
        _instance = this;
        _ = GetNoel();
    }

    private async Task GetNoel()
    {
        //make sure to get the node when it's loaded
        while (Noel == null || !Noel.IsInsideTree())
        {
            Noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
            noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
    public Vector3 GetNoelPosition()
    {
        if (Noel != null)
        {
            noelPosition = Noel.GlobalPosition;
        }
        return noelPosition;
    }
    public void setnoelMoves(bool move)
    {
        noelMoving = !move;
    }
}
