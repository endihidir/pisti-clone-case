
public interface ICardBehaviour
{
    public int CardIndex { get; }
    public int CardNumber { get; }
    public bool IsPistiCard { get; set; }
    public CardType CardType { get; }
    public ICardInputDetector CardInputDetector { get; set; }
    public ICardAnimationService CardAnimationService { get; set; }
    public void Initialize(int cardIndex, int cardNumber, CardType cardType);
    public int GetCardPoint();
}