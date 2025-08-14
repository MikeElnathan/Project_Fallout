using Godot;
using System;

public partial class PlayerAnimation : AnimationPlayer
{
    private static AnimationPlayer _animInstance;
    public static AnimationPlayer Anim_Instance => _animInstance;
    public override void _Ready()
    {
        if (_animInstance != null && _animInstance != this)
        {
            QueueFree();
            return;
        }
        _animInstance = this;
    }
}
