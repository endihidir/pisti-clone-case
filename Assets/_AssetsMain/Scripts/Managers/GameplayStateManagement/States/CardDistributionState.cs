using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class CardDistributionState : IState
{
    public event Action OnStateComplete;

    private bool _isFirstRound = true, _isAllCardsFinished;
    public bool IsAllCardsFinished => _isAllCardsFinished;

    private readonly ICardContainer _cardContainer;
    private readonly IUserDeck _playerDeck;
    private readonly IUserDeck[] _opponentDecks;
    private readonly IDiscardDeck _discardDeck;

    public CardDistributionState(ICardContainer cardContainer, IUserDeck playerDeck, IUserDeck[] opponetsDeck, IDiscardDeck discardDeck)
    {
        _cardContainer = cardContainer;
        _playerDeck = playerDeck;
        _opponentDecks = opponetsDeck;
        _discardDeck = discardDeck;
    }
    
    public async void OnEnter()
    {
        await DistributeToDiscardDeck();

        await DistributePlayerCards();

        await DistributeOpponentsCards();
    }

    private async UniTask DistributeToDiscardDeck()
    {
        if (_isFirstRound)
        {
            _isFirstRound = false;

            var tasks = new UniTask[4];

            for (int i = 0; i < 4; i++)
            {
                var canGet = _cardContainer.TryGetRandomCard(out var cardViewController);

                if (canGet)
                {
                    var cardAnimationService = cardViewController.CardBehaviour.CardAnimationService;
                    if (i > 2) cardAnimationService.Flip(CardFace.Front, 0.25f, Ease.InOutQuad, i * 0.15f);
                    tasks[i] = cardAnimationService.Move(_discardDeck.Slots[0].position, 0.5f, Ease.InOutQuad, i * 0.15f);
                }
            }

            await UniTask.WhenAll(tasks);
        }
    }
    
    private async UniTask DistributePlayerCards()
    {
        var tasks = new UniTask[4];
        
        for (int i = 0; i < _playerDeck.Slots.Length; i++)
        {
            var canGet = _cardContainer.TryGetRandomCard(out var cardViewController);

            if (canGet)
            {
                var cardAnimationService = cardViewController.CardBehaviour.CardAnimationService;
                cardAnimationService.Flip(CardFace.Front, 0.25f, Ease.InOutQuad, i * 0.15f);
                cardAnimationService.Rotate(_playerDeck.Slots[i].rotation, 0.5f, Ease.InOutQuad, i * 0.15f);
                tasks[i] = cardAnimationService.Move(_playerDeck.Slots[i].position, 0.5f, Ease.InOutQuad, i * 0.15f);
            }
        }

        await UniTask.WhenAll(tasks);
    }
    
    private async UniTask DistributeOpponentsCards()
    {
        var tasks = new UniTask[4];

        foreach (var opponent in _opponentDecks)
        {
            for (int i = 0; i < opponent.Slots.Length; i++)
            {
                var canGet = _cardContainer.TryGetRandomCard(out var cardViewController);

                if (canGet)
                {
                    var cardAnimationService = cardViewController.CardBehaviour.CardAnimationService;
                    cardAnimationService.Rotate(opponent.Slots[i].rotation, 0.5f, Ease.InOutQuad, i * 0.15f);
                    tasks[i] = cardAnimationService.Move(opponent.Slots[i].position, 0.5f, Ease.InOutQuad, i * 0.15f);
                }
            }
        }

        await UniTask.WhenAll(tasks);
    }

    public void OnUpdate(float deltaTime)
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Reset()
    {
        _isFirstRound = true;
    }
}