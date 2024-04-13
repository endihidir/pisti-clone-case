using System.Collections.Generic;
using UnityBase.ManagerSO;
using UnityEngine;

public class DiscardPileController : IDiscardPile
{
    private readonly Stack<ICardBehaviour> _dealtCards;
    private readonly Transform[] _slots;
    private int _lastCollectedUserID = -1;
    public Stack<ICardBehaviour> DealtCards => _dealtCards;
    public Transform[] Slots => _slots;
    public int LastCollectedUserID => _lastCollectedUserID;

    public DiscardPileController(GameplayDataHolderSO gameplayDataHolderSo)
    {
        _slots = gameplayDataHolderSo.gameplayStateMachineSo.GetBoardView<DiscardPileBoardView>().Slots;
        
        _dealtCards = new Stack<ICardBehaviour>();
    }

    public void PushCard(ICardBehaviour cardBehaviour) => _dealtCards.Push(cardBehaviour);

    public CardCollectingType GetCard(ICardBehaviour cardBehaviour)
    {
        var hasCard = _dealtCards.TryPeek(out var lastCardBeforeDrop);
        
        PushCard(cardBehaviour);

        if (hasCard)
        {
            var isNumberedCardsSame = IsNumberedCardsSame(lastCardBeforeDrop, cardBehaviour);
            var isSpecialCardsSame = IsSpecialCardsSame(lastCardBeforeDrop, cardBehaviour);
            
            if (isNumberedCardsSame || isSpecialCardsSame)
            {
                if (_dealtCards.Count == 2)
                {
                    cardBehaviour.IsPistiCard = true;
                    _lastCollectedUserID = cardBehaviour.OwnerUserID;
                    return CardCollectingType.Pisti;
                }

                _lastCollectedUserID = cardBehaviour.OwnerUserID;
                return CardCollectingType.CollectAll;
            }

            if(cardBehaviour is JackCard)
            {
                _lastCollectedUserID = cardBehaviour.OwnerUserID;
                return CardCollectingType.CollectAll;
            }
        }

        return CardCollectingType.None;
    }
    private bool IsNumberedCardsSame(ICardBehaviour lastCardBeforeDrop, ICardBehaviour cardBehaviour)
    {
        return lastCardBeforeDrop is NumberedCard 
               && cardBehaviour is NumberedCard && 
               lastCardBeforeDrop.CardNumber == cardBehaviour.CardNumber;
    }

    private bool IsSpecialCardsSame(ICardBehaviour lastCardBeforeDrop, ICardBehaviour cardBehaviour)
    {
        return lastCardBeforeDrop is SpecialCard 
               && cardBehaviour is SpecialCard && 
               lastCardBeforeDrop.GetType() == cardBehaviour.GetType();
    }

    public bool IsCardMatchedWith(ICardBehaviour cardBehaviour)
    {
        var lastCard = _dealtCards.Peek();
        
        return cardBehaviour.GetType() == lastCard.GetType();
    }
    
    public void ClearDeck() => _dealtCards.Clear();
}

public enum CardCollectingType
{
    None,
    CollectAll,
    Pisti,
}