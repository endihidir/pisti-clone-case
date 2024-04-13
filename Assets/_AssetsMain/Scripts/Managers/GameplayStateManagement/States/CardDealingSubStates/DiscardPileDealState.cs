using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class DiscardPileDealState : IState
{
    private bool _isFirstRound = true;
    
    public event Action OnStateComplete;

    private readonly IDiscardPile _discardPile;
    
    private readonly ICardContainer _cardContainer;
    public DiscardPileDealState(IDiscardPile discardPile, ICardContainer cardContainer)
    {
        _discardPile = discardPile;
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

        for (int i = 0; i < CardConstants.DEALING_COUNT; i++)
        {
            if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
            
            _discardPile.PushCard(cardBehaviour);
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var delay = i * CardConstants.MOVE_DELAY;
            if (i > 2) cardAnimationService.Flip(CardFace.Front, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
            cardAnimationService.Move(_discardPile.Slots[0].position, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
        }

        await UniTask.WaitForSeconds(CardConstants.MOVE_DURATION + CardConstants.MOVE_DELAY);
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }

    public void Reset()
    {
        _isFirstRound = false;
    }
}