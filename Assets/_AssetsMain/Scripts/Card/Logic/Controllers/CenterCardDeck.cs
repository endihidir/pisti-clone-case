using System.Collections.Generic;

public class CenterCardDeck : ICenterCardDeck
{
    protected readonly Stack<ICardBehaviour> _droppedCards;
    
    public CenterCardDeck()
    {
        _droppedCards = new Stack<ICardBehaviour>();
    }

    public void Initialize()
    {
        
    }
    
    public void Visit<T>(T visitable) where T : ICardBehaviour
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

public interface ICenterCardDeck
{
    public void Initialize();
    
    public void Visit<T>(T visitable) where T : ICardBehaviour;
}