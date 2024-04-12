using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class CardDistributionState : IState
{ 
    public event Action OnStateComplete;

    private bool _isFirstRound = true;
    private readonly ICardContainer _cardContainer;
    private readonly IUserBoard _playerBoard;
    private readonly IUserBoard[] _opponentBoards;
    private readonly IDiscardBoard _discardBoard;
    private float _stateCompleteDelay;

    public CardDistributionState(ICardContainer cardContainer, IUserBoard playerBoard, IUserBoard[] opponetBoards, IDiscardBoard discardBoard)
    {
        _cardContainer = cardContainer;
        _playerBoard = playerBoard;
        _opponentBoards = opponetBoards;
        _discardBoard = discardBoard;
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
        else
        {
            await DistributeRaminingCardsToLastCollectedUser();
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
            
            _discardBoard.PushCard(cardBehaviour);
            
            var cardAnimationService = cardBehaviour.CardAnimationService;
            if (i > 2) cardAnimationService.Flip(CardFace.Front, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
            cardAnimationService.Move(_discardBoard.Slots[0].position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
        }

        await UniTask.WaitForSeconds(CardConstants.DISTRIBUTION_SPEED + 0.1f);
    }
    
    private async UniTask DistributeToPlayerDeck()
    {
        _stateCompleteDelay += CardConstants.DISTRIBUTION_DELAY / 3f;
        
        for (int i = 0; i < CardConstants.DISTRIBUTION_COUNT; i++)
        {
            if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
            
            cardBehaviour.OwnerUserID = _playerBoard.UserID;
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
        
        foreach (var opponent in _opponentBoards)
        {
            for (int i = 0; i < CardConstants.DISTRIBUTION_COUNT; i++)
            {
                if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
                
                cardBehaviour.OwnerUserID = opponent.UserID;
                var opponentDeck = opponent.UserDeck;
                opponentDeck.AddCard(cardBehaviour);
                var cardAnimationService = cardBehaviour.CardAnimationService;
                cardAnimationService.Rotate(opponentDeck.Slots[i].rotation, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
                cardAnimationService.Move(opponentDeck.Slots[i].position, CardConstants.DISTRIBUTION_SPEED, Ease.InOutQuad, i * CardConstants.DISTRIBUTION_DELAY);
            }
            
            await UniTask.WaitForSeconds(CardConstants.DISTRIBUTION_SPEED + 0.1f);
        }
    }
    
    private async UniTask DistributeRaminingCardsToLastCollectedUser()
    {
        if (_discardBoard.DroppedCards.Count > 0)
        {
            var distributionSpeed = CardConstants.DISTRIBUTION_SPEED;
            var distributionDelay = CardConstants.DISTRIBUTION_DELAY;
            var droppedCards = _discardBoard.DroppedCards;

            var tasks = new List<UniTask>();
            var index = 0;
            var collectedCards = SelectLastCardCollectedBoard().CollectedCards;
            
            foreach (var cardBehaviour in droppedCards)
            {
                var cardAnimationService = cardBehaviour.CardAnimationService;
                collectedCards.CollectCard(cardBehaviour);
                cardAnimationService.Flip(CardFace.Back, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
                cardAnimationService.Rotate(collectedCards.CardCollectingArea.rotation, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
                var task = cardAnimationService.Move(collectedCards.CardCollectingArea.position, distributionSpeed, Ease.InOutQuad, index * distributionDelay);
                tasks.Add(task);
                index++;
            }

            await UniTask.WhenAll(tasks);

            _discardBoard.ClearDeck();
        }
    }

    private IUserBoard SelectLastCardCollectedBoard()
    {
        return _discardBoard.LastCollectedUserID == _playerBoard.UserID ? _playerBoard : 
            _opponentBoards.FirstOrDefault(opponentBoard => opponentBoard.UserID == _discardBoard.LastCollectedUserID);
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { _stateCompleteDelay = 0f; }
    public void Reset() { _isFirstRound = true; }
}