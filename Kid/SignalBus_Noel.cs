using Godot;
using System;

public partial class SignalBus_Noel : Node3D
{
    private static SignalBus_Noel _instance_noel;
    public static SignalBus_Noel Instance_noel => _instance_noel;

    public override void _Ready()
    {
        if (_instance_noel != null && _instance_noel != this)
        {
            QueueFree();
            return;
        }
        _instance_noel = this;
    }
    public void EmitNoelSignal()
    {
        
    }
}
