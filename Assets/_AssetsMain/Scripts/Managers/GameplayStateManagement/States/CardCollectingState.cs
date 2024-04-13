using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class CardCollectingState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _collectedUserBoard;
    private readonly IDiscardBoard _discardBoard;

    public CardCollectingState(IUserBoard collectedUserBoard, IDiscardBoard discardBoard)
    {
        _collectedUserBoard = collectedUserBoard;
        _discardBoard = discardBoard;
    }
    
    public async void OnEnter()
    {
        await CollectAllCards();
        
        OnStateComplete?.Invoke();
    }
    
    private async UniTask CollectAllCards()
    {
        var distributionSpeed = CardConstants.MOVE_SPEED;
        var droppedCards = _discardBoard.DroppedCards;

        var tasks = new UniTask[droppedCards.Count];
        var index = 0;
        
        foreach (var cardBehaviour in droppedCards)
        {
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var collectedCards = _collectedUserBoard.CollectedCards;
            collectedCards.CollectCard(cardBehaviour);
            cardBehaviour.OwnerUserID = _collectedUserBoard.UserID;
            
            var distributionDelay = index * CardConstants.MOVE_DELAY;
            cardAnimationService.Flip(CardFace.Back, distributionSpeed, Ease.InOutQuad, distributionDelay);
            cardAnimationService.Rotate(collectedCards.CardCollectingArea.rotation, distributionSpeed, Ease.InOutQuad, distributionDelay);
            var task = cardAnimationService.Move(collectedCards.CardCollectingArea.position, distributionSpeed, Ease.InOutQuad, distributionDelay);

            tasks[index] = task;
            index++;
        }
        
        await UniTask.WhenAll(tasks);
        
        _discardBoard.ClearDeck();
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}