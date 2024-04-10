using System;
using UnityBase.StateMachineCore;

public class PlayerMoveState : IState
{
    public event Action OnStateComplete;
    
    public IUserDeck PlayerDeck { get; }
    public int CardCount => PlayerDeck.CardBehaviours.Count;
    public bool IsCardsFinished => CardCount < 1;

    public PlayerMoveState(IUserDeck playerDeck)
    {
        PlayerDeck = playerDeck;
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