using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class PlayerCardDealState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard _playerBoard;
    
    private readonly ICardContainer _cardContainer;
    public PlayerCardDealState(IUserBoard playerBoard, ICardContainer cardContainer)
    {
        _playerBoard = playerBoard;
        _cardContainer = cardContainer;
    }
    
    public async void OnEnter()
    {
        await DealCardsToPlayerDeck();

        OnStateComplete?.Invoke();
    }
    
    private async UniTask DealCardsToPlayerDeck()
    {
        for (int i = 0; i < CardConstants.DEALING_COUNT; i++)
        {
            if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
            
            cardBehaviour.OwnerUserID = _playerBoard.UserID;
            var playerDeck = _playerBoard.UserDeck;
            playerDeck.AddCard(cardBehaviour);
            var cardAnimationService = cardBehaviour.CardAnimationService;
            var delay = i * CardConstants.MOVE_DELAY;
            cardAnimationService.Flip(CardFace.Front, CardConstants.MOVE_SPEED, Ease.InOutQuad, delay);
            cardAnimationService.Rotate(playerDeck.Slots[i].rotation, CardConstants.MOVE_SPEED, Ease.InOutQuad, delay);
            cardAnimationService.Move(playerDeck.Slots[i].position, CardConstants.MOVE_SPEED, Ease.InOutQuad, delay);
        }

        await UniTask.WaitForSeconds(CardConstants.MOVE_SPEED + CardConstants.MOVE_DELAY);
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}