using System;
using UnityBase.StateMachineCore;

public class IdleState : IState
{
    public event Action OnStateComplete;
    public void OnEnter() { }
    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}