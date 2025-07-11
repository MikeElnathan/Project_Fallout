using Godot;
using System;

public partial class NoelCharacterStats : CharacterStat
{
    private string fileName = "NoelSaveData";
    public float NoelMood { get; private set; } = 100f;
    private float MaxMood = 100f;
    public override void _Ready()
    {
        base._Ready();
        //load your save file here
    }
    public void ModMood(float modifier, Operation operation)
    {
        switch (operation)
        {
            case Operation.Multiply:
                NoelMood *= modifier;
                break;
            case Operation.Add:
                NoelMood += modifier;
                break;
            case Operation.Substract:
                NoelMood -= modifier;
                break;
        }
        NoelMood = Mathf.Clamp(NoelMood, 0f, MaxMood);
    }
}
