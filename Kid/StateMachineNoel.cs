using Godot;
using System;

public partial class StateMachineNoel : BaseStateMachine
{
    public static CharacterBody3D Noel;

    public override void _Ready()
    {
        base._Ready();
        changeState("FollowPlayer");
    }

}
