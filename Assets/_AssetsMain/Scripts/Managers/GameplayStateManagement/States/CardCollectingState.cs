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
        var moveSpeed = CardConstants.MOVE_SPEED;
        var droppedCards = _discardPile.DealtCards;

        var tasks = new UniTask[droppedCards.Count];
        var index = 0;
        
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var collectedCards = _collectedUserBoard.CollectedCards;
            collectedCards.CollectCard(cardBehaviour);
            cardBehaviour.OwnerUserID = _collectedUserBoard.UserID;
            
            var distributionDelay = index * CardConstants.MOVE_DELAY;
            cardAnimationService.Flip(CardFace.Back, moveSpeed, Ease.InOutQuad, distributionDelay);
            cardAnimationService.Rotate(collectedCards.CardCollectingArea.rotation, moveSpeed, Ease.InOutQuad, distributionDelay);
            var task = cardAnimationService.Move(collectedCards.CardCollectingArea.position, moveSpeed, Ease.InOutQuad, distributionDelay);

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