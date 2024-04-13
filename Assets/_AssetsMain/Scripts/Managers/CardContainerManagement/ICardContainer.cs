public interface ICardContainer
{
    public int TotalDeckCount { get; }
    public int TotalCardCount { get; }
    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour);
    public bool IsAllCardsFinished();
}
