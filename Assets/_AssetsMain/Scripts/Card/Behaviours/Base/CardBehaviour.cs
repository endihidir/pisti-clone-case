using UnityEngine;

public abstract class CardBehaviour : ICardBehaviour
{
    public int CardIndex { get; private set; }
    public int CardNumber { get; private set; }
    public CardType CardType { get; private set; }
    public Sprite CardSprite { get; private set; }
    public ICardAnimationService CardAnimationService { get; set; }

    public void Initialize(int cardIndex, int cardNumber, CardType cardType, Sprite cardSprite)
    {
        CardIndex = cardIndex;
        CardNumber = cardNumber;
        CardType = cardType;
        CardSprite = cardSprite;
    }

    public abstract int GetCardPoint();
}