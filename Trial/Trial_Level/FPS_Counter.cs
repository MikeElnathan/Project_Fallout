using Godot;

public partial class FPS_Counter : Label
{
    public override void _Ready()
    {
        base._Ready();
        GD.Print($"TRIAL_1 READY: {GetPath()}");
    }

    public override void _Process(double delta)
    {
        Text = $"FPS: {Engine.GetFramesPerSecond()}";
    }

}
