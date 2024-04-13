using System.Collections.Generic;
using UnityEngine;

public interface IDiscardPile
{
    public Stack<ICardBehaviour> DealtCards { get; }
    public Transform[] Slots { get; }
    public int LastCollectedUserID { get; }
    public void PushCard(ICardBehaviour cardBehaviour);
    public CardCollectingType GetCard(ICardBehaviour cardBehaviour);
    public bool IsCardMatchedWith(ICardBehaviour cardBehaviour);
    public void ClearDeck();
}