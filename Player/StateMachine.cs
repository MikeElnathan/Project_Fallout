using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node3D
{
    private State currentState;
    private StateMachine _stateMachine;
    private SignalBus signalBus;
    private Dictionary<string, State> _states = new();

    public override void _Ready()
    {
        signalBus = SignalBus.Instance;
        ReadSignal();
    }

    private void GetAllStates()
    {
        foreach (Node3D child in GetChildren())
        {
            if (child is State state)
            {
                _states[child.Name] = state;
                state.Owner = GetParent();
                state._stateMachine = this;
            }
        }

        if (_states.Count == 0)
        {
            //warning here
        }
    }

    private void changeState(string name)
    {
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
