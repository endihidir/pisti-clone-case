
public interface ICardBehaviour
{
    public int CardIndex { get; }
    public int CardNumber { get; }
    public CardType CardType { get; }
    public int OwnerUserID { get; set; }
    public bool IsPistiCard { get; set; }
    public ICardInputDetectionService CardInputDetectionService { get; set; }
    public ICardAnimationService CardAnimationService { get; set; }
    public void Initialize(int cardIndex, int cardNumber, CardType cardType);
    public int GetCardPoint();
}