﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityBase.StateMachineCore;

public class OpponentCardDealState : IState
{
    public event Action OnStateComplete;

    private readonly IUserBoard[] _opponentBoards;
    
    private readonly ICardContainer _cardContainer;
    public OpponentCardDealState(IUserBoard[] opponentBoards, ICardContainer cardContainer)
    {
        _opponentBoards = opponentBoards;
        _cardContainer = cardContainer;
    }
    
    public async void OnEnter()
    {
        await DealCardsToOpponentDeck();
            
        await UniTask.WaitForSeconds(0.15f);
        
        OnStateComplete?.Invoke();
    }
    
    private async UniTask DealCardsToOpponentDeck()
    {
        foreach (var opponent in _opponentBoards)
        {
            for (int i = 0; i < CardConstants.DEALING_COUNT; i++)
            {
                if (!_cardContainer.TryGetRandomCard(out var cardBehaviour)) continue;
                
                cardBehaviour.OwnerUserID = opponent.UserID;
                var opponentDeck = opponent.UserDeck;
                opponentDeck.AddCard(cardBehaviour);
                var cardAnimationService = cardBehaviour.CardAnimationService;
                var delay = i * CardConstants.MOVE_DELAY;
                cardAnimationService.Rotate(opponentDeck.Slots[i].rotation, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
                cardAnimationService.Move(opponentDeck.Slots[i].position, CardConstants.MOVE_DURATION, Ease.InOutQuad, delay);
            }
            
            await UniTask.WaitForSeconds(CardConstants.MOVE_DURATION + CardConstants.MOVE_DELAY);
        }
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
}