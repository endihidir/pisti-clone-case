using System;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class OpponentMoveState : IState
{
    public event Action OnStateComplete;
    private readonly IUserBoard _opponentBoard;
    private readonly IDiscardPile _discardPile;
    private readonly IState _cardCollectingState;
    
    public int UserID => _opponentBoard.UserID;
    public int CardCount => _opponentBoard.UserDeck.CardBehaviours.Count;
    public bool IsCardsFinished => CardCount < 1;

    public OpponentMoveState(IUserBoard opponentBoard, IDiscardPile discardPile)
    {
        _opponentBoard = opponentBoard;
        _discardPile = discardPile;
        
        _cardCollectingState = new CardCollectingState(opponentBoard, discardPile);
        _cardCollectingState.OnStateComplete += OnCardCollectingStateComplete;
    }

    public async void OnEnter()
    {
        var opponentDeck = _opponentBoard.UserDeck;
        
        if (opponentDeck.TryGetRandomCard(out var cardBehaviour))
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            
            cardAnimationService.Flip(CardFace.Front, CardConstants.MOVE_SPEED, Ease.InOutQuad);
            cardAnimationService.Rotate(_discardPile.Slots[0].rotation, CardConstants.MOVE_SPEED, Ease.InOutQuad);
            await cardAnimationService.Move(_discardPile.Slots[0].position, CardConstants.MOVE_SPEED, Ease.InOutQuad);
            OnDropComplete(cardBehaviour);
        }
        else
        {
            OnStateComplete?.Invoke();
        }
    }
    
    private void OnDropComplete(ICardBehaviour cardBehaviour)
    {
        var collectingType = _discardPile.GetCard(cardBehaviour);
        _opponentBoard.UserDeck.DropCard(cardBehaviour);

        switch (collectingType)
        {
            case CardCollectingType.None:
                OnStateComplete?.Invoke();
                break;
            case CardCollectingType.CollectAll:
            case CardCollectingType.Pisti:
                _cardCollectingState.OnEnter();
                break;
        }
    }
    
    private void OnCardCollectingStateComplete() => OnStateComplete?.Invoke();
    public void OnUpdate(float deltaTime) => _cardCollectingState.OnUpdate(deltaTime);
    public void OnExit() => _cardCollectingState.OnExit();
    
    public void Reset()
    {
        _cardCollectingState.Reset();
        _cardCollectingState.OnStateComplete -= OnCardCollectingStateComplete;
    }
}