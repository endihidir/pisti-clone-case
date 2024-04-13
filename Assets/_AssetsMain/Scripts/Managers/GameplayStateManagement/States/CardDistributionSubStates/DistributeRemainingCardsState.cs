using System;
using System.Linq;
using UnityBase.StateMachineCore;

public class DistributeRemainingCardsState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _playerBoard;
    private readonly IUserBoard[] _opponentsBoard;
    private readonly IDiscardBoard _discardBoard;
    
    private IState _cardCollectingState;
    public DistributeRemainingCardsState(IUserBoard playerBoard, IUserBoard[] opponentsBoard, IDiscardBoard discardBoard)
    {
        _playerBoard = playerBoard;
        _opponentsBoard = opponentsBoard;
        _discardBoard = discardBoard;
    }
    
    public void OnEnter() => DistributeRaminingCardsToLastCollectedUser();

    private void DistributeRaminingCardsToLastCollectedUser()
    {
        if (_discardBoard.DroppedCards.Count > 0)
        {
            var lastCollectedBoard = SelectLastCardCollectedBoard();

            _cardCollectingState = new CardCollectingState(lastCollectedBoard, _discardBoard);
            
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
        return _discardBoard.LastCollectedUserID == _playerBoard.UserID ? _playerBoard : 
            _opponentsBoard.FirstOrDefault(opponentBoard => opponentBoard.UserID == _discardBoard.LastCollectedUserID);
    }

    public void OnUpdate(float deltaTime) => _cardCollectingState?.OnUpdate(deltaTime);
    public void OnExit() => _cardCollectingState?.OnExit();
    public void Reset() { }
}