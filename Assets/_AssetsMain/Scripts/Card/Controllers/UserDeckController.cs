using System.Collections.Generic;
using UnityEngine;

public class UserDeckController : IUserDeck
{
    public int UserID { get;}
    
    private readonly List<CardBehaviour> _cardBehaviours;
    private readonly Transform[] _slots;
    
    public List<CardBehaviour> CardBehaviours => _cardBehaviours;
    
    public Transform[] Slots => _slots;
    public UserDeckController(int userID, Transform[] slots)
    {
        UserID = userID;
        _slots = slots;
        _cardBehaviours = new List<CardBehaviour>();
    }

    public void AddCard(CardBehaviour cardBehaviour)
    {
        _cardBehaviours.Add(cardBehaviour);
    }

    public void DropCardTo(int index, IDiscardDeck visitor)
    {
        visitor.Visit(UserID, _cardBehaviours[index]);
        
        _cardBehaviours.RemoveAt(index);
    }
}