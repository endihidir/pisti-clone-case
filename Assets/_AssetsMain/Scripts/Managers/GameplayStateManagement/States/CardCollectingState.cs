using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class CardCollectingState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _collectedUserBoard;
    private readonly IDiscardPile _discardPile;

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

        var tasks = new UniTask[droppedCards.Count];
        var index = 0;
        
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var collectedCards = _collectedUserBoard.CollectedCards;
            collectedCards.CollectCard(cardBehaviour);
            cardBehaviour.OwnerUserID = _collectedUserBoard.UserID;
            
            var delay = index * CardConstants.MOVE_DELAY;
            cardAnimationService.Flip(CardFace.Back, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
            cardAnimationService.Rotate(collectedCards.CardCollectingArea.rotation, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
            var task = cardAnimationService.Move(collectedCards.CardCollectingArea.position, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);

            tasks[index] = task;
            index++;
        }
        
        await UniTask.WhenAll(tasks);
        
        _discardPile.ClearDeck();
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}