using System.Collections.Generic;
using UnityEngine;

public interface IDiscardBoard
{
    public Stack<ICardBehaviour> DroppedCards { get; }
    public Transform[] Slots { get; }
    public int LastCollectedUserID { get; }
    public void PushCard(ICardBehaviour cardBehaviour);
    public CardCollectingType GetCard(ICardBehaviour cardBehaviour);
    public void ClearDeck();
}