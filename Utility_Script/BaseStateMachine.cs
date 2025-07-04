using Godot;
using Godot.Collections;
using System;

public partial class BaseStateMachine : Node
{
    protected State currentState;
    public BaseStateMachine _stateMachine { get; set; }
    protected Dictionary<string, State> _states = new();
    protected AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        ReadSignal();
        GetAllStates();
    }

    protected virtual void GetAnimation()
    {
        //your method here
    }
    protected virtual void GetAllStates()
    {
        GetAnimation();
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _states[child.Name] = state;
                state.Owner = GetParent();
                state._stateMachine = this;
                state.InjectDependencies(animationPlayer);
            }
        //GD.Print("States: ", child.Name);
        }
    }

    protected virtual void changeState(string name)
    {
        if (currentState?.Name == name) return;

        if (_states.TryGetValue(name, out var newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
        else
        {
            GD.PushWarning($"State {name}, not found.");
        }
    }
    protected virtual void ReadSignal()
    {
        //implement your method
    }
    public override void _Process(double delta)
    {
        currentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState?.PhysicUpdate(delta);
    }
}
