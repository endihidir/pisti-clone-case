using System.Collections.Generic;

public abstract class BaseCardDeck : IBaseCardDeck
{
    protected readonly Stack<ICardBehaviour> _cardBehaviours;

    private ICardBehaviour _cardBehaviour;
    
    public BaseCardDeck() => _cardBehaviours = new Stack<ICardBehaviour>();

    public void SelectCard(ICardBehaviour cardBehaviour) => _cardBehaviour = cardBehaviour;

    public void DropCardTo(ICenterCardDeck visitor)
    {
        if(_cardBehaviour is null) return;
        
        visitor.Visit(_cardBehaviour);
    }
}