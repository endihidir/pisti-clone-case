using System;
using UnityBase.StateMachineCore;
using UnityEngine;

public class ResultCalculationState : IState
{
    public event Action OnStateComplete;

    private bool _isPlayerWin;

    public bool IsPlayerWin => _isPlayerWin;

    public ResultCalculationState()
    {
        
    }
    
    public void OnEnter()
    {
        Debug.Log("a");
    }

    public void OnUpdate(float deltaTime)
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Reset()
    {
        
    }
}