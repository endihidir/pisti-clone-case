using System;
using UnityBase.StateMachineCore;

public class CardDistributionState : IState
{ 
    public event Action OnStateComplete;
    
    private readonly IState _discardDistributionState, _playerCardDistributionState, _opponentsCardDistributionState, _remainingCardsDistributionState;
    
    private readonly ICardContainer _cardContainer;

    public CardDistributionState(ICardContainer cardContainer, IUserBoard playerBoard, IUserBoard[] opponetBoards, IDiscardBoard discardBoard)
    {
        _cardContainer = cardContainer;
        
        _discardDistributionState = new DiscardDistributionState(discardBoard, _cardContainer);
        _discardDistributionState.OnStateComplete += OnDiscardDistributionStateComplete;

        _playerCardDistributionState = new PlayerCardDistributionState(playerBoard, _cardContainer);
        _playerCardDistributionState.OnStateComplete += OnPlayerCardDistributionStateComplete;

        _opponentsCardDistributionState = new OpponentCardDistributionState(opponetBoards, _cardContainer);
        _opponentsCardDistributionState.OnStateComplete += OnOpponentCardDistributionStateComplete;

        _remainingCardsDistributionState = new DistributeRemainingCardsState(playerBoard, opponetBoards, discardBoard);
        _remainingCardsDistributionState.OnStateComplete += OnRemainingCardsDistributionDistributionStateComplete;
    }

    public void OnEnter()
    {
        if (!_cardContainer.IsAllCardsFinished())
        {
            _discardDistributionState.OnEnter();
        }
        else
        {
            _remainingCardsDistributionState.OnEnter();
        }
    }

    private void OnDiscardDistributionStateComplete() => _playerCardDistributionState.OnEnter();
    private void OnPlayerCardDistributionStateComplete() => _opponentsCardDistributionState.OnEnter();
    private void OnOpponentCardDistributionStateComplete() => OnStateComplete?.Invoke();
    private void OnRemainingCardsDistributionDistributionStateComplete() => OnStateComplete?.Invoke();

    public void OnUpdate(float deltaTime)
    {
        _discardDistributionState.OnUpdate(deltaTime);
        _playerCardDistributionState.OnUpdate(deltaTime);
        _opponentsCardDistributionState.OnUpdate(deltaTime);
        _remainingCardsDistributionState.OnUpdate(deltaTime);
    }

    public void OnExit()
    {
        _discardDistributionState.OnExit();
        _playerCardDistributionState.OnExit();
        _opponentsCardDistributionState.OnExit();
        _remainingCardsDistributionState.OnExit();
    }

    public void Reset()
    {
        _discardDistributionState.Reset();
        _playerCardDistributionState.Reset();
        _opponentsCardDistributionState.Reset();
        _remainingCardsDistributionState.Reset();
        
        _discardDistributionState.OnStateComplete -= OnDiscardDistributionStateComplete;
        _playerCardDistributionState.OnStateComplete -= OnPlayerCardDistributionStateComplete;
        _opponentsCardDistributionState.OnStateComplete -= OnOpponentCardDistributionStateComplete;
        _remainingCardsDistributionState.OnStateComplete -= OnRemainingCardsDistributionDistributionStateComplete;
    }
}