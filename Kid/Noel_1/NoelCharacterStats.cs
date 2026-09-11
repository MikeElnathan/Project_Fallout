using Godot;
using System;

public partial class NoelCharacterStats : CharacterStat
{
    private string fileName = "NoelSaveData";
    public override void _Ready()
    {
        base._Ready();
        //load your save file here
    }
}
