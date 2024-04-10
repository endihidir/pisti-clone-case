public interface ICardContainer
{
    public bool TryGetRandomCard(out CardViewController cardViewController);
    public bool TryGetCardBy(int index, out CardViewController cardViewController);
    public bool IsNumberedCard(CardType cardType);
}
