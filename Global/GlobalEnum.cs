using Godot;
using System;

public partial class GlobalEnum : Node
{
    public enum State
    {
        Idle, Walk, Run, Jump, Sleep, Sneak
    }
    public enum Focus
    {
        Player, Noel
    }
}
