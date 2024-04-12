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
    private readonly IDiscardDeck _discardDeck;
    private readonly Camera _cam;

    private bool _isCardSelected;
    public PlayerMoveState(IUserBoard playerBoard, IDiscardDeck discardDeck)
    {
        _playerBoard = playerBoard;
        _discardDeck = discardDeck;
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
                    var cardAnimationService = cardBehaviour.CardAnimationService;
                    cardAnimationService.Rotate(_discardDeck.Slots[0].rotation, 0.5f, Ease.InOutQuad);
                    await cardAnimationService.Move(_discardDeck.Slots[0].position, 0.5f, Ease.InOutQuad);
                    OnDropComplete(cardBehaviour);
                    break;
                }
            }
        }
    }

    private void OnDropComplete(ICardBehaviour cardBehaviour)
    {
        var collectingType = _discardDeck.GetCard(cardBehaviour);
        
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
        var droppedCards = _discardDeck.DroppedCards;
        
        var tasks = new List<UniTask>();
        var index = 0;
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var collectedCards = _playerBoard.CollectedCards;
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

    public void OnExit() => _isCardSelected = false;
    public void Reset() { }
}