using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : Enum
{
    private Dictionary<T, State> _states = new();
    private State _currentState;

    public void RegisterState(T type, State state)
    {
        if (_states.ContainsKey(type))
            throw new Exception("Stato già presente: " + type);

        _states.Add(type, state);
    }

    public void SetState(T type)
    {
        if (!_states.ContainsKey(type))
            throw new Exception("Stato non registrato: " + type);

        _currentState?.OnExit();
        _currentState = _states[type];
        _currentState.OnEnter();
    }

    public State GetCurrentState()
    {
        if (_currentState == null) return null;

        return _currentState;
    }

    public void Update() => _currentState?.OnUpdate();

}
