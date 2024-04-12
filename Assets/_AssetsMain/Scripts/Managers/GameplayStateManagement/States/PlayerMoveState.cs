using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;
using UnityEngine;

public class PlayerMoveState : IState
{
    public event Action OnStateComplete;
    private readonly IUserBoard _playerBoard;
    private readonly IDiscardBoard _discardBoard;
    private readonly Camera _cam;

    private bool _isCardSelected;
    public PlayerMoveState(IUserBoard playerBoard, IDiscardBoard discardBoard)
    {
        _playerBoard = playerBoard;
        _discardBoard = discardBoard;
        _cam = Camera.main;
    }
    
    public void OnEnter() { }

    public void OnUpdate(float deltaTime) => SelectCard();

    private async void SelectCard()
    {
        if (Input.GetMouseButtonUp(0) && !_isCardSelected)
        {
            var playerDeck = _playerBoard.UserDeck;
            
            for (int i = playerDeck.CardBehaviours.Count - 1; i >= 0; i--)
            {
                var cardBehaviour = playerDeck.CardBehaviours[i];

                var inputDetector = cardBehaviour.CardInputDetectionService;

                if (inputDetector.IsInRange(Input.mousePosition))
                {
                    _isCardSelected = true;
                    var distributionSpeed = CardConstants.DISTRIBUTION_SPEED;
                    var cardAnimationService = cardBehaviour.CardAnimationService;
                    cardAnimationService.Rotate(_discardBoard.Slots[0].rotation, distributionSpeed, Ease.InOutQuad);
                    await cardAnimationService.Move(_discardBoard.Slots[0].position, distributionSpeed, Ease.InOutQuad);
                    OnDropComplete(cardBehaviour);
                    break;
                }
            }
        }
    }

    private void OnDropComplete(ICardBehaviour cardBehaviour)
    {
        var collectingType = _discardBoard.GetCard(cardBehaviour);
        
        _playerBoard.UserDeck.DropCard(cardBehaviour);
        
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
            var collectedCards = _playerBoard.CollectedCards;
            collectedCards.CollectCard(cardBehaviour);
            cardAnimationService.Flip(CardFace.Back, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
            cardAnimationService.Rotate(collectedCards.CardCollectingArea.rotation, distributionSpeed, Ease.InOutQuad, index *distributionDelay);
            var task = cardAnimationService.Move(collectedCards.CardCollectingArea.position, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
            tasks.Add(task);
            index++;
        }
        
        await UniTask.WhenAll(tasks);
        
        _discardBoard.ClearDeck();
        
        OnStateComplete?.Invoke();
    }

    public void OnExit() => _isCardSelected = false;
    public void Reset() { }
}