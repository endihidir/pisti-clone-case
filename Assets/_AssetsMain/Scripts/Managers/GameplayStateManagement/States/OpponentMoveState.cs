using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class OpponentMoveState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _opponentBoard;
    private readonly IDiscardBoard _discardBoard;
    public int UserID => _opponentBoard.UserID;
    public int CardCount => _opponentBoard.UserDeck.CardBehaviours.Count;
    public bool IsCardsFinished => CardCount < 1;

    public OpponentMoveState(IUserBoard opponentBoard, IDiscardBoard discardBoard)
    {
        _opponentBoard = opponentBoard;
        _discardBoard = discardBoard;
    }
    
    public async void OnEnter()
    {
        var distributionSpeed = CardConstants.DISTRIBUTION_SPEED;
        var opponentDeck = _opponentBoard.UserDeck;
        
        if (opponentDeck.TryGetRandomCard(out var cardBehaviour))
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            cardAnimationService.Flip(CardFace.Front, distributionSpeed, Ease.InOutQuad);
            cardAnimationService.Rotate(_discardBoard.Slots[0].rotation, distributionSpeed, Ease.InOutQuad);
            await cardAnimationService.Move(_discardBoard.Slots[0].position, distributionSpeed, Ease.InOutQuad);
            OnDropComplete(cardBehaviour);
        }
        else
        {
            OnStateComplete?.Invoke();
        }
    }
    
    private void OnDropComplete(ICardBehaviour cardBehaviour)
    {
        var collectingType = _discardBoard.GetCard(cardBehaviour);
        
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
        var distributionSpeed = CardConstants.DISTRIBUTION_SPEED;
        var distributionDelay = CardConstants.DISTRIBUTION_DELAY;
        var droppedCards = _discardBoard.DroppedCards;
        
        var tasks = new List<UniTask>();
        var index = 0;
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var collectedCards = _opponentBoard.CollectedCards;
            collectedCards.CollectCard(cardBehaviour);
            cardAnimationService.Flip(CardFace.Back, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
            cardAnimationService.Rotate(collectedCards.CardCollectingArea.rotation, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
            var task = cardAnimationService.Move(collectedCards.CardCollectingArea.position, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
            tasks.Add(task);
            index++;
        }
        
        await UniTask.WhenAll(tasks);
        
        _discardBoard.ClearDeck();
        
        OnStateComplete?.Invoke();
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}