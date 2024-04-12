using System.Collections.Generic;
using UnityBase.ManagerSO;
using UnityEngine;

public class DiscardDeckController : IDiscardDeck
{
    private readonly Stack<ICardBehaviour> _droppedCards;
    private readonly Transform[] _slots;
    public Stack<ICardBehaviour> DroppedCards => _droppedCards;
    public Transform[] Slots => _slots;
    
    public DiscardDeckController(GameplayDataHolderSO gameplayDataHolderSo)
    {
        _slots = gameplayDataHolderSo.gameplayStateMachineSo.GetDeckView<DiscardBoardView>().Slots;
        
        _droppedCards = new Stack<ICardBehaviour>();
    }

    public void PushCard(ICardBehaviour cardBehaviour) => _droppedCards.Push(cardBehaviour);

    public CardCollectingType GetCard(ICardBehaviour cardBehaviour)
    {
        var hasCard = _droppedCards.TryPeek(out var lastCardBeforeDrop);
        
        PushCard(cardBehaviour);

        if (hasCard)
        {
            var isNumberedCardsSame = IsNumberedCardsSame(lastCardBeforeDrop, cardBehaviour);
            var isSpecialCardsSame = IsSpecialCardsSame(lastCardBeforeDrop, cardBehaviour);
            
            if (isNumberedCardsSame || isSpecialCardsSame)
            {
                if (_droppedCards.Count == 2)
                {
                    cardBehaviour.IsPistiCard = true;
                    return CardCollectingType.Pisti;
                }

                return CardCollectingType.CollectAll;
            }

            if(cardBehaviour is JackCard)
            {
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
    
    public void ClearDeck() => _droppedCards.Clear();
}

public enum CardCollectingType
{
    None,
    CollectAll,
    Pisti,
}