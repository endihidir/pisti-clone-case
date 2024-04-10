using System;
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
        if (_isFirstRound)
        {
            _isFirstRound = false;
            
            for (int i = 0; i < 4; i++)
            {
                var canGet = _cardContainer.TryGetRandomCard(out var cardViewController);

                if (canGet)
                {
                    var cardBehaviour = cardViewController.CardBehaviour;
                    
                    var cardAnimationService = cardBehaviour.CardAnimationService;
                    
                    if (i > 2)
                    {
                        cardAnimationService.Flip(CardFace.Front, 0.2f, Ease.InOutQuad);
                    }
                    
                    await cardAnimationService.MoveToTargetLocal(_discardDeck.Slots[0], 0.4f, Ease.InOutQuad);
                }
            }
        }
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