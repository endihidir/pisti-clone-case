using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class OpponentMoveState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _opponentBoard;
    private readonly IDiscardDeck _discardDeck;
    public int UserID => _opponentBoard.UserID;
    public int CardCount => _opponentBoard.UserDeck.CardBehaviours.Count;
    public bool IsCardsFinished => CardCount < 1;

    public OpponentMoveState(IUserBoard opponentBoard, IDiscardDeck discardDeck)
    {
        _opponentBoard = opponentBoard;
        _discardDeck = discardDeck;
    }
    
    public async void OnEnter()
    {
        var opponentDeck = _opponentBoard.UserDeck;
        
        if (opponentDeck.TryGetRandomCard(out var cardBehaviour))
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            cardAnimationService.Flip(CardFace.Front, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, CardConstants.DISTRIBUTION_DELAY);
            cardAnimationService.Rotate(_discardDeck.Slots[0].rotation, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, CardConstants.DISTRIBUTION_DELAY);
            await cardAnimationService.Move(_discardDeck.Slots[0].position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, CardConstants.DISTRIBUTION_DELAY);
            OnDropComplete(cardBehaviour);
        }
        else
        {
            OnStateComplete?.Invoke();
        }
    }
    
    private void OnDropComplete(ICardBehaviour cardBehaviour)
    {
        var collectingType = _discardDeck.GetCard(cardBehaviour);
        
        _opponentBoard.UserDeck.DropCard(cardBehaviour);

        switch (collectingType)
        {
            case CardCollectingType.None:
                OnStateComplete?.Invoke();
                break;
            case CardCollectingType.CollectAll:
            case CardCollectingType.Pisti:
                CollectAllCards();
                break;
        }
    }

    private async void CollectAllCards()
    {
        var droppedCards = _discardDeck.DroppedCards;
        
        var tasks = new List<UniTask>();
        var index = 0;
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var collectedCards = _opponentBoard.CollectedCards;
            collectedCards.CollectCard(cardBehaviour);
            cardAnimationService.Flip(CardFace.Back, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, index * CardConstants.DISTRIBUTION_DELAY);
            cardAnimationService.Rotate(collectedCards.CollectedCardPoint.rotation, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, index * CardConstants.DISTRIBUTION_DELAY);
            var task = cardAnimationService.Move(collectedCards.CollectedCardPoint.position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, index * CardConstants.DISTRIBUTION_DELAY);
            tasks.Add(task);
            index++;
        }
        
        await UniTask.WhenAll(tasks);
        
        _discardDeck.ClearDeck();
        
        OnStateComplete?.Invoke();
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}