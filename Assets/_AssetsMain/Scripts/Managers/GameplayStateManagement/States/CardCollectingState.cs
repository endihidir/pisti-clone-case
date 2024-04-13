using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class CardCollectingState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _collectedUserBoard;
    private readonly IDiscardPile _discardPile;
    private ICollectedCards _collectedCards;
    public CardCollectingState(IUserBoard collectedUserBoard, IDiscardPile discardPile)
    {
        _collectedUserBoard = collectedUserBoard;
        _discardPile = discardPile;
    }
    
    public async void OnEnter()
    {
        await CollectAllCards();
        
        OnStateComplete?.Invoke();
    }
    
    private async UniTask CollectAllCards()
    {
        var droppedCards = _discardPile.DealtCards;
        _collectedCards = _collectedUserBoard.CollectedCards;

        var tasks = new UniTask[droppedCards.Count];
        var index = 0;
        
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            _collectedCards.CollectCard(cardBehaviour);
            cardBehaviour.OwnerUserID = _collectedUserBoard.UserID;
            
            var delay = index * CardConstants.MOVE_DELAY;
            cardAnimationService.Flip(CardFace.Back, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
            cardAnimationService.Rotate(_collectedCards.CardCollectingArea.rotation, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
            var task = cardAnimationService.Move(_collectedCards.CardCollectingArea.position, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay, OnMovementComplete);
            tasks[index] = task;
            index++;
        }

        await UniTask.WhenAll(tasks);
        
        _collectedCards.UpdateTotalCardPointView();
        
        _discardPile.ClearDeck();
    }

    private void OnMovementComplete() => _collectedCards.UpdateCollectedCardCountView();

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}