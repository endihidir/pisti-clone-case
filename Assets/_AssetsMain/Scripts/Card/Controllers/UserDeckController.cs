using System.Collections.Generic;
using System.Linq;
using UnityBase.Extensions;
using UnityEngine;

public class UserDeckController : IUserDeck
{
    private readonly List<ICardBehaviour> _cardBehaviours;
    private readonly Transform[] _slots;
    
    public List<ICardBehaviour> CardBehaviours => _cardBehaviours;
    public Transform[] Slots => _slots;
    
    public UserDeckController(Transform[] slots)
    {
        _slots = slots;
        _cardBehaviours = new List<ICardBehaviour>();
    }

    public void AddCard(ICardBehaviour cardBehaviour) => _cardBehaviours.Add(cardBehaviour);
    public void DropCard(ICardBehaviour cardBehaviour) => _cardBehaviours.Remove(cardBehaviour);
    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour)
    {
        var firstCard =  _cardBehaviours.Shuffle().FirstOrDefault();

        cardBehaviour = null;
        
        if (firstCard != null)
        {
            cardBehaviour = firstCard;
            _cardBehaviours.Remove(firstCard);
            return true;
        }
        
        return false;
    }

    public bool ContainsCard(ICardBehaviour cardBehaviour) => _cardBehaviours.Contains(cardBehaviour);
}