using System;
using DG.Tweening;
using UnityBase.StateMachineCore;
using UnityEngine;

public class PlayerMoveState : IState
{
    public event Action OnStateComplete;
    private readonly IUserBoard _playerBoard;
    private readonly IDiscardPile _discardPile;
    private readonly IState _cardCollectingState;
    private bool _isCardSelected;
    
    public PlayerMoveState(IUserBoard playerBoard, IDiscardPile discardPile)
    {
        _playerBoard = playerBoard;
        _discardPile = discardPile;

        _cardCollectingState = new CardCollectingState(playerBoard, discardPile);
        _cardCollectingState.OnStateComplete += OnCardCollectingStateComplete;
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
                    var distributionSpeed = CardConstants.MOVE_SPEED;
                    var cardAnimationService = cardBehaviour.CardAnimationService;
                    cardAnimationService.Rotate(_discardPile.Slots[0].rotation, distributionSpeed, Ease.InOutQuad);
                    await cardAnimationService.Move(_discardPile.Slots[0].position, distributionSpeed, Ease.InOutQuad);
                    OnDropComplete(cardBehaviour);
                    break;
                }
            }
        }
    }

    private void OnDropComplete(ICardBehaviour cardBehaviour)
    {
        var collectingType = _discardPile.GetCard(cardBehaviour);
        _playerBoard.UserDeck.DropCard(cardBehaviour);
        
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
    public void OnExit()
    {
        _cardCollectingState.OnExit();
        _isCardSelected = false;
    }

    public void Reset()
    {
        _cardCollectingState.Reset();
        _cardCollectingState.OnStateComplete -= OnCardCollectingStateComplete;
    }
}