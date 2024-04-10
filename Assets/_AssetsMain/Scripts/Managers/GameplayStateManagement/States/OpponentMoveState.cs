using System;
using UnityBase.StateMachineCore;

public class OpponentMoveState : IState
{
    public event Action OnStateComplete;

    private readonly IUserDeck _opponentDeck;
    public int UserID => _opponentDeck.UserID;
    public int CardCount => _opponentDeck.CardBehaviours.Count;
    public bool IsCardsFinished => CardCount < 1;

    public OpponentMoveState(IUserDeck opponentDeck)
    {
        _opponentDeck = opponentDeck;
    }
    
    public void OnEnter()
    {
        
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