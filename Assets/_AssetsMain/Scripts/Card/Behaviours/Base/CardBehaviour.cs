
public abstract class CardBehaviour : ICardBehaviour
{
    public int CardIndex { get; private set; }
    public int CardNumber { get; private set; }
    public bool IsPistiCard { get; set; }
    public CardType CardType { get; private set; }
    public int OwnerUserID { get; set; }
    public ICardInputDetectionService CardInputDetectionService { get; set; }
    public ICardAnimationService CardAnimationService { get; set; }

    public void Initialize(int cardIndex, int cardNumber, CardType cardType)
    {
        CardIndex = cardIndex;
        CardNumber = cardNumber;
        CardType = cardType;
    }

    public abstract int GetCardPoint();
}