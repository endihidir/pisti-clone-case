using System.Collections.Generic;
using UnityEngine;

public interface IUserDeck
{
   public int UserID { get; }
   public List<CardBehaviour> CardBehaviours { get; }
   public Transform[] Slots { get; }
   public void AddCard(CardBehaviour cardBehaviour);
   public void DropCardTo(int index, IDiscardDeck visitor);
}