using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class CardDistributionState : IState
{ 
    public event Action OnStateComplete;

    private bool _isFirstRound = true;
    public bool IsAllCardsFinished => _opponentDecks[^1].UserDeck.CardBehaviours.Count < 1;

    private readonly ICardContainer _cardContainer;
    private readonly IUserBoard _playerBoard;
    private readonly IUserBoard[] _opponentDecks;
    private readonly IDiscardDeck _discardDeck;
    private float _stateCompleteDelay;

    public CardDistributionState(ICardContainer cardContainer, IUserBoard playerBoard, IUserBoard[] opponetsDeck, IDiscardDeck discardDeck)
    {
        _cardContainer = cardContainer;
        _playerBoard = playerBoard;
        _opponentDecks = opponetsDeck;
        _discardDeck = discardDeck;
    }
    
    public async void OnEnter()
    {
        if (!_cardContainer.IsAllCardsFinished())
        {
            await DistributeToDiscardDeck();
            await DistributeToPlayerDeck();
            await DistributeToOpponentDeck();
            await UniTask.WaitForSeconds(_stateCompleteDelay);
        }
        
        OnStateComplete?.Invoke();
    }

    private async UniTask DistributeToDiscardDeck()
    {
        if (!_isFirstRound) return;
        
        _isFirstRound = false;

        _stateCompleteDelay += CardConstants.DISTRIBUTION_DELAY / 3f;
        
        for (int i = 0; i < CardConstants.DISTRIBUTION_COUNT; i++)
        {
            if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
            
            _discardDeck.PushCard(cardBehaviour);
            
            var cardAnimationService = cardBehaviour.CardAnimationService;
            if (i > 2) cardAnimationService.Flip(CardFace.Front, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
            cardAnimationService.Move(_discardDeck.Slots[0].position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
        }

        await UniTask.WaitForSeconds(CardConstants.DISTRIBUTION_SPEED + 0.1f);
    }
    
    private async UniTask DistributeToPlayerDeck()
    {
        _stateCompleteDelay += CardConstants.DISTRIBUTION_DELAY / 3f;
        
        for (int i = 0; i < CardConstants.DISTRIBUTION_COUNT; i++)
        {
            if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;

            var playerDeck = _playerBoard.UserDeck;
            playerDeck.AddCard(cardBehaviour);
            
            var cardAnimationService = cardBehaviour.CardAnimationService;
            cardAnimationService.Flip(CardFace.Front, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
            cardAnimationService.Rotate(playerDeck.Slots[i].rotation, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
            cardAnimationService.Move(playerDeck.Slots[i].position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
        }

        await UniTask.WaitForSeconds(CardConstants.DISTRIBUTION_SPEED + 0.1f);
    }
    
    private async UniTask DistributeToOpponentDeck()
    {
        _stateCompleteDelay += CardConstants.DISTRIBUTION_DELAY / 3f;
        
        foreach (var opponent in _opponentDecks)
        {
            for (int i = 0; i < CardConstants.DISTRIBUTION_COUNT; i++)
            {
                if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
                
                var opponentDeck = opponent.UserDeck;
                opponentDeck.AddCard(cardBehaviour);
                
                var cardAnimationService = cardBehaviour.CardAnimationService;
                cardAnimationService.Rotate(opponentDeck.Slots[i].rotation, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
                cardAnimationService.Move(opponentDeck.Slots[i].position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
            }
            
            await UniTask.WaitForSeconds(CardConstants.DISTRIBUTION_SPEED + 0.1f);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        
    }

    public void OnExit()
    {
        _stateCompleteDelay = 0f;
    }

    public void Reset()
    {
        _isFirstRound = true;
    }
}