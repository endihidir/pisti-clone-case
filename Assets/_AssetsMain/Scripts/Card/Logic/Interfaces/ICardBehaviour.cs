using UnityEngine;

public interface ICardBehaviour
{
    public CardType CardType { get; }
    public int CardNumber { get; }
    public Sprite CardSprite { get; }
    public void Initialize(int cardNumber, CardType cardType, Sprite cardSprite);
    public int GetCardPoint();
    public void Accept(ICenterCardDeck visitor);
}