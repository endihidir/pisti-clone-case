using UnityEngine;

public interface ICardBehaviour
{
    public int CardIndex { get; }
    public int CardNumber { get; }
    public CardType CardType { get; }
    public Sprite CardSprite { get; }
    public ICardAnimationService CardAnimationService { get; set; }
    public void Initialize(int cardIndex, int cardNumber, CardType cardType, Sprite cardSprite);
    public int GetCardPoint();
}