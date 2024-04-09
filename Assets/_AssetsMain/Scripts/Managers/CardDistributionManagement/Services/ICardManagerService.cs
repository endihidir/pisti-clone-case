public interface ICardManagerService
{
    public bool TryGetCardObject(out CardViewController cardViewController);
    public bool IsNumberedCard(CardType cardType);
}
