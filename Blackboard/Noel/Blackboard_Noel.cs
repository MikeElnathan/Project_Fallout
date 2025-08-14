using Godot;
using System.Threading.Tasks;

public partial class Blackboard_Noel : Node
{
    private CharacterBody3D _Noel;
    private Noel _noel;
    public GlobalEnum.State noelCurrentState { get; set; }
    public Vector3 noelPosition { get; private set; }
    public Vector3 noelVelocity { get; private set; }
    public GlobalEnum.State NoelState { get; }
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
        while (_Noel == null || !_Noel.IsInsideTree())
        {
            _Noel = GetTree().GetFirstNodeInGroup("Noel") as CharacterBody3D;
            _noel = GetTree().GetFirstNodeInGroup("Noel") as Noel;
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
    public Vector3 GetNoelPosition()
    {
        if (_Noel != null)
        {
            noelPosition = _Noel.GlobalPosition;
        }
        return noelPosition;
    }
}
