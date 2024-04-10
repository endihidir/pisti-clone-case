using UnityEngine;

public interface IDiscardDeck
{
    public Transform[] Slots { get; }
    public void Visit<T>(int userID, T visitable) where T : ICardBehaviour;
}