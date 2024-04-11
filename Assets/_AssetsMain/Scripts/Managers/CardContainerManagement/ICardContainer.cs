public interface ICardContainer
{
    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour);
    public bool IsAllCardsFinished();
}
