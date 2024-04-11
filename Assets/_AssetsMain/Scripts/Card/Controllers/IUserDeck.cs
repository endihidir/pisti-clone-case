using System.Collections.Generic;
using UnityEngine;

public interface IUserDeck
{
    public List<ICardBehaviour> CardBehaviours { get; }
    public Transform[] Slots { get; }
    public void AddCard(ICardBehaviour cardBehaviour);
    public void DropCard(ICardBehaviour cardBehaviour);
    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour);
    public bool ContainsCard(ICardBehaviour cardBehaviour);
}