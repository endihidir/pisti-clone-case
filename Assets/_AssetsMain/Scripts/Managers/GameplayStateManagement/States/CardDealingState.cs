using System;
using UnityBase.StateMachineCore;

public class CardDealingState : IState
{ 
    public event Action OnStateComplete;
    
    private readonly IState _discardDealState, _playerCardDealState, _opponentsCardDealState, _remainingCardsDealState;
    
    private readonly ICardContainer _cardContainer;

    public CardDealingState(ICardContainer cardContainer, IUserBoard playerBoard, IUserBoard[] opponetBoards, IDiscardPile discardPile)
    {
        _cardContainer = cardContainer;
        
        _discardDealState = new DiscardPileDealState(discardPile, _cardContainer);
        _discardDealState.OnStateComplete += OnDiscardDealStateComplete;

        _playerCardDealState = new PlayerCardDealState(playerBoard, _cardContainer);
        _playerCardDealState.OnStateComplete += OnPlayerCardDealStateComplete;

        _opponentsCardDealState = new OpponentCardDealState(opponetBoards, _cardContainer);
        _opponentsCardDealState.OnStateComplete += OnOpponentCardDealStateComplete;

        _remainingCardsDealState = new DealRemainingCardsState(playerBoard, opponetBoards, discardPile);
        _remainingCardsDealState.OnStateComplete += OnRemainingCardsDealDealStateComplete;
    }

    public void OnEnter()
    {
        if (!_cardContainer.IsAllCardsFinished())
        {
            _discardDealState.OnEnter();
        }
        else
        {
            _remainingCardsDealState.OnEnter();
        }
    }

    private void OnDiscardDealStateComplete() => _playerCardDealState.OnEnter();
    private void OnPlayerCardDealStateComplete() => _opponentsCardDealState.OnEnter();
    private void OnOpponentCardDealStateComplete() => OnStateComplete?.Invoke();
    private void OnRemainingCardsDealDealStateComplete() => OnStateComplete?.Invoke();

    public void OnUpdate(float deltaTime)
    {
        _discardDealState.OnUpdate(deltaTime);
        _playerCardDealState.OnUpdate(deltaTime);
        _opponentsCardDealState.OnUpdate(deltaTime);
        _remainingCardsDealState.OnUpdate(deltaTime);
    }

    public void OnExit()
    {
        _discardDealState.OnExit();
        _playerCardDealState.OnExit();
        _opponentsCardDealState.OnExit();
        _remainingCardsDealState.OnExit();
    }

    public void Reset()
    {
        _discardDealState.Reset();
        _playerCardDealState.Reset();
        _opponentsCardDealState.Reset();
        _remainingCardsDealState.Reset();

        _discardDealState.OnStateComplete -= OnDiscardDealStateComplete;
        _playerCardDealState.OnStateComplete -= OnPlayerCardDealStateComplete;
        _opponentsCardDealState.OnStateComplete -= OnOpponentCardDealStateComplete;
        _remainingCardsDealState.OnStateComplete -= OnRemainingCardsDealDealStateComplete;
    }
}