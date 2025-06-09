using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node3D
{
    private State currentState;
    public StateMachine _stateMachine { get; set; }
    private SignalBus signalBus;
    private Dictionary<string, State> _states = new();

    public override void _Ready()
    {
        signalBus = SignalBus.Instance;
        ReadSignal();
        GetAllStates();
    }

    private void GetAllStates()
    {
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _states[child.Name] = state;
                state.Owner = GetParent();
                state._stateMachine = this;
            }
        }

        GD.Print(_states);
    }

    private void changeState(string name)
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
    private void ReadSignal()
    {
        signalBus.Walk  += () => changeState("Walk");
        signalBus.Run   += () => changeState("Run");
        signalBus.Idle  += () => changeState("Idle");
        signalBus.Jump  += () => changeState("Jump");
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
