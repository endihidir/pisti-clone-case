using System;
using System.Linq;
using UnityBase.StateMachineCore;

public class DealRemainingCardsState : IState
{
    public event Action OnStateComplete;
    private readonly IUserBoard _playerBoard;
    private readonly IUserBoard[] _opponentsBoard;
    private readonly IDiscardPile _discardPile;
    private IState _cardCollectingState;
    
    public DealRemainingCardsState(IUserBoard playerBoard, IUserBoard[] opponentsBoard, IDiscardPile discardPile)
    {
        _playerBoard = playerBoard;
        _opponentsBoard = opponentsBoard;
        _discardPile = discardPile;
    }
    
    public void OnEnter() => DealRaminingCardsToLastCollectedUser();

    private void DealRaminingCardsToLastCollectedUser()
    {
        if (_discardPile.DealtCards.Count > 0)
        {
            var lastCollectedBoard = SelectLastCardCollectedBoard();

            _cardCollectingState = new CardCollectingState(lastCollectedBoard, _discardPile);
            
            _cardCollectingState.OnStateComplete += OnCardCollectingStateComplete;
            
            _cardCollectingState.OnEnter();
        }
        else
        {
            OnStateComplete?.Invoke();
        }
    }

    private void OnCardCollectingStateComplete()
    {
        _cardCollectingState.Reset();
        
        _cardCollectingState.OnStateComplete -= OnCardCollectingStateComplete;

        OnStateComplete?.Invoke();
    }

    private IUserBoard SelectLastCardCollectedBoard()
    {
        return _discardPile.LastCollectedUserID == _playerBoard.UserID ? _playerBoard : 
            _opponentsBoard.FirstOrDefault(opponentBoard => opponentBoard.UserID == _discardPile.LastCollectedUserID);
    }

    public void OnUpdate(float deltaTime) => _cardCollectingState?.OnUpdate(deltaTime);
    public void OnExit() => _cardCollectingState?.OnExit();
    public void Reset() { }
}