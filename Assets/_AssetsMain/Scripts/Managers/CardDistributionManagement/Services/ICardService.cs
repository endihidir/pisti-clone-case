public interface ICardService
{
    public bool TryGetCardObject(out CardViewController cardViewController);
    public bool IsNumberedCard(CardType cardType);
}
