using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public StateChange[] states;
    public EnemyAgent agent;
    public EnemyState currentState;

    public EnemyStateMachine(EnemyAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(EnemyState)).Length;
        states = new StateChange[numStates];
    }

    public void RegisterState(StateChange state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public StateChange GetState(EnemyState state)
    {
        int index = (int)state;
        return states[index];
    }

    public void ChangeState(EnemyState newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }
}
