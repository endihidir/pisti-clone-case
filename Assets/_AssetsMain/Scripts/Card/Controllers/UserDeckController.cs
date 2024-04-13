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
    
    public UserDeckController(UserBoardView userBoardView)
    {
        _slots = userBoardView.Slots;
        _cardBehaviours = new List<ICardBehaviour>();
    }

    public void AddCard(ICardBehaviour cardBehaviour) => _cardBehaviours.Add(cardBehaviour);
    public void DropCard(ICardBehaviour cardBehaviour) => _cardBehaviours.Remove(cardBehaviour);
    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour)
    {
        cardBehaviour =  _cardBehaviours.Shuffle().FirstOrDefault();

        return cardBehaviour != null;
    }

    public bool ContainsCard(ICardBehaviour cardBehaviour) => _cardBehaviours.Contains(cardBehaviour);
    public void Reset()
    {
        _cardBehaviours.Clear();
    }
}