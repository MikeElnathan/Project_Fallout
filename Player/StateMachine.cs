using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node3D
{
    private State currentState;
    private Dictionary<string, State> states = new();

    public void addState(string name, State state)
    {
        if (!states.ContainsKey(name))
        {
            states.Add(name, state);
        }
    }
    public void changeState(string name)
    {
        if (!states.ContainsKey(name)) { return; }

        currentState?.Exit();
        currentState = states[name];
        currentState.Enter();
    }

    public override void _Process(double delta)
    {
        currentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState.PhysicUpdate(delta);
    }

}
