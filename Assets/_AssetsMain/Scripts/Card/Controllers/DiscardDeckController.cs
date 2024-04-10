using System.Collections.Generic;
using UnityBase.ManagerSO;
using UnityEngine;

public class DiscardDeckController : IDiscardDeck
{
    private readonly Stack<ICardBehaviour> _droppedCards;
    private readonly Transform[] _slots;

    public Transform[] Slots => _slots;
    
    public DiscardDeckController(GameplayDataHolderSO gameplayDataHolderSo)
    {
        _slots = gameplayDataHolderSo.gameplayStateMachineSo.GetDeckView<DiscardView>().Slots;
        
        _droppedCards = new Stack<ICardBehaviour>();
    }

    public void Visit<T>(int userId, T visitable) where T : ICardBehaviour
    {
        _droppedCards.Push(visitable);

        //visitable.CardAnimationService.Move();  // OnComplete decide collecting situation
        
        DecideCollecting(userId, visitable);
    }

    private void DecideCollecting<T>(int userId, T visitable) where T : ICardBehaviour
    {
        if (visitable.GetType() == GetLastCard().GetType())
        {
            if (_droppedCards.Count == 1)
            {
                
            }
            else
            {
                
            }
        }
        else
        {
        }
    }

    private ICardBehaviour GetLastCard() => _droppedCards.Peek();
}