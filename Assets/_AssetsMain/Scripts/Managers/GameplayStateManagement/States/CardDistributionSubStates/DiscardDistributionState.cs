using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class DiscardDistributionState : IState
{
    private bool _isFirstRound = true;
    
    public event Action OnStateComplete;

    private readonly IDiscardBoard _discardBoard;
    
    private readonly ICardContainer _cardContainer;
    public DiscardDistributionState(IDiscardBoard discardBoard, ICardContainer cardContainer)
    {
        _discardBoard = discardBoard;
        _cardContainer = cardContainer;
    }
    
    public async void OnEnter()
    {
        await DistributeToDiscardDeck();
        
        OnStateComplete?.Invoke();
    }
    
    private async UniTask DistributeToDiscardDeck()
    {
        if (!_isFirstRound) return;
        
        _isFirstRound = false;

        for (int i = 0; i < CardConstants.DISTRIBUTION_COUNT; i++)
        {
            if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
            
            _discardBoard.PushCard(cardBehaviour);
            
            var cardAnimationService = cardBehaviour.CardAnimationService;
            if (i > 2) cardAnimationService.Flip(CardFace.Front, CardConstants.MOVE_SPEED, Ease.InOutQuad, i * CardConstants.MOVE_DELAY);
            cardAnimationService.Move(_discardBoard.Slots[0].position, CardConstants.MOVE_SPEED, Ease.InOutQuad, i * CardConstants.MOVE_DELAY);
        }

        await UniTask.WaitForSeconds(CardConstants.MOVE_SPEED + CardConstants.MOVE_DELAY);
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }

    public void Reset()
    {
        _isFirstRound = false;
    }
}