using UnityEngine;

public abstract class CardBehaviour : ICardBehaviour
{
    public int CardNumber { get; protected set; }
    public CardType CardType { get; protected set; }
    public Sprite CardSprite { get; protected set; }
    public ICardAnimationService CardAnimationService { get; set; }

    public void Initialize(int cardNumber, CardType cardType, Sprite cardSprite)
    {
        CardNumber = cardNumber;
        CardType = cardType;
        CardSprite = cardSprite;
    }

    public abstract int GetCardPoint();
    
    public void Accept(ICenterCardDeck visitor) => visitor.Visit(this);
}