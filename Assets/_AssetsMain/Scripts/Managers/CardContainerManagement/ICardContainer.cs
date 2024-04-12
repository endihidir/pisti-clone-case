public interface ICardContainer
{
    public int TotalDeckCount { get; }
    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour);
    public bool IsAllCardsFinished();
}
